import AsyncStorage from '@react-native-async-storage/async-storage';
import * as LocalAuthentication from 'expo-local-authentication';
import React, { createContext, useCallback, useContext, useEffect, useMemo, useReducer, useState } from 'react';
import { api } from '../services/api';
import { ProfilePayload, User } from '../types';

const AUTH_TOKEN_KEY = 'authToken';
const BIOMETRIC_SETTINGS_KEY = '@carea:biometric-settings';

type BiometricPreferenceKey = 'faceIdEnabled' | 'fingerprintEnabled';

type BiometricSettings = {
  faceIdEnabled: boolean;
  fingerprintEnabled: boolean;
};

type BiometricCapabilities = {
  hasHardware: boolean;
  isEnrolled: boolean;
  supportsFaceId: boolean;
  supportsFingerprint: boolean;
};

interface AuthState {
  user: User | null;
  token: string | null;
  isGuest: boolean;
  isLoading: boolean;
}

type AuthAction =
  | { type: 'SET_LOADING'; payload: boolean }
  | { type: 'LOGIN'; payload: { user: User; token: string } }
  | { type: 'SET_GUEST' }
  | { type: 'LOGOUT' }
  | { type: 'UPDATE_USER'; payload: User };

const initialState: AuthState = {
  user: null,
  token: null,
  isGuest: false,
  isLoading: true,
};

const defaultBiometricSettings: BiometricSettings = {
  faceIdEnabled: false,
  fingerprintEnabled: false,
};

const defaultBiometricCapabilities: BiometricCapabilities = {
  hasHardware: false,
  isEnrolled: false,
  supportsFaceId: false,
  supportsFingerprint: false,
};

function reducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'SET_LOADING':
      return { ...state, isLoading: action.payload };
    case 'LOGIN':
      return { user: action.payload.user, token: action.payload.token, isGuest: false, isLoading: false };
    case 'SET_GUEST':
      return { user: null, token: null, isGuest: true, isLoading: false };
    case 'LOGOUT':
      return { user: null, token: null, isGuest: false, isLoading: false };
    case 'UPDATE_USER':
      return { ...state, user: action.payload };
    default:
      return state;
  }
}

async function getBiometricCapabilitiesAsync(): Promise<BiometricCapabilities> {
  const [hasHardware, isEnrolled, supportedTypes] = await Promise.all([
    LocalAuthentication.hasHardwareAsync(),
    LocalAuthentication.isEnrolledAsync(),
    LocalAuthentication.supportedAuthenticationTypesAsync(),
  ]);

  return {
    hasHardware,
    isEnrolled,
    supportsFaceId: supportedTypes.includes(LocalAuthentication.AuthenticationType.FACIAL_RECOGNITION),
    supportsFingerprint: supportedTypes.includes(LocalAuthentication.AuthenticationType.FINGERPRINT),
  };
}

function isBiometricEnabled(settings: BiometricSettings) {
  return settings.faceIdEnabled || settings.fingerprintEnabled;
}

function getBiometricLabel(settings: BiometricSettings, capabilities: BiometricCapabilities) {
  if (settings.faceIdEnabled && capabilities.supportsFaceId) {
    return 'Face ID';
  }

  if (settings.fingerprintEnabled && capabilities.supportsFingerprint) {
    return 'Fingerprint';
  }

  if (capabilities.supportsFaceId) {
    return 'Face ID';
  }

  if (capabilities.supportsFingerprint) {
    return 'Fingerprint';
  }

  return 'Biometrics';
}

async function authenticateWithDevice(promptMessage: string) {
  const result = await LocalAuthentication.authenticateAsync({
    promptMessage,
    cancelLabel: 'Cancel',
    fallbackLabel: 'Use device passcode',
  });

  return result.success;
}

interface AuthContextValue extends AuthState {
  login: (
    userNameOrEmail: string,
    password: string,
    options?: { rememberMe?: boolean }
  ) => Promise<{ needsProfile?: boolean; userId?: string }>;
  signup: (payload: { email: string; password: string; fullName: string; phoneNumber: string }) => Promise<void>;
  fillProfile: (userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) => Promise<void>;
  continueAsGuest: () => void;
  logout: () => Promise<void>;
  updateProfile: (payload: ProfilePayload) => Promise<void>;
  biometricSettings: BiometricSettings;
  biometricCapabilities: BiometricCapabilities;
  biometricLabel: string | null;
  canLoginWithBiometrics: boolean;
  refreshBiometricCapabilities: () => Promise<BiometricCapabilities>;
  setBiometricPreference: (
    key: BiometricPreferenceKey,
    enabled: boolean
  ) => Promise<{ enabled: boolean; message?: string }>;
  loginWithBiometrics: () => Promise<boolean>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(reducer, initialState);
  const [biometricSettings, setBiometricSettings] = useState<BiometricSettings>(defaultBiometricSettings);
  const [biometricCapabilities, setBiometricCapabilities] = useState<BiometricCapabilities>(defaultBiometricCapabilities);
  const [hasStoredToken, setHasStoredToken] = useState(false);

  const refreshBiometricCapabilities = useCallback(async () => {
    const capabilities = await getBiometricCapabilitiesAsync();
    setBiometricCapabilities(capabilities);
    return capabilities;
  }, []);

  const loadBiometricSettings = useCallback(async () => {
    try {
      const raw = await AsyncStorage.getItem(BIOMETRIC_SETTINGS_KEY);
      if (!raw) {
        setBiometricSettings(defaultBiometricSettings);
        return defaultBiometricSettings;
      }

      const parsed = JSON.parse(raw) as Partial<BiometricSettings>;
      const nextSettings = {
        faceIdEnabled: Boolean(parsed.faceIdEnabled),
        fingerprintEnabled: Boolean(parsed.fingerprintEnabled),
      };

      setBiometricSettings(nextSettings);
      return nextSettings;
    } catch {
      setBiometricSettings(defaultBiometricSettings);
      return defaultBiometricSettings;
    }
  }, []);

  const restoreSessionFromToken = useCallback(async (token: string) => {
    const { user } = await api.getMe();
    dispatch({ type: 'LOGIN', payload: { user, token } });
    setHasStoredToken(true);
    return true;
  }, []);

  const loginWithBiometrics = useCallback(async (settingsOverride?: BiometricSettings) => {
    const token = await AsyncStorage.getItem(AUTH_TOKEN_KEY);
    if (!token) {
      setHasStoredToken(false);
      return false;
    }

    const settings = settingsOverride ?? biometricSettings;
    if (!isBiometricEnabled(settings)) {
      return false;
    }

    const capabilities = await refreshBiometricCapabilities();
    const label = getBiometricLabel(settings, capabilities);
    if (!capabilities.hasHardware || !capabilities.isEnrolled || !label) {
      return false;
    }

    const success = await authenticateWithDevice(`Login with ${label}`);
    if (!success) {
      return false;
    }

    try {
      await restoreSessionFromToken(token);
      return true;
    } catch {
      await AsyncStorage.removeItem(AUTH_TOKEN_KEY);
      setHasStoredToken(false);
      dispatch({ type: 'LOGOUT' });
      return false;
    }
  }, [biometricSettings, refreshBiometricCapabilities, restoreSessionFromToken]);

  useEffect(() => {
    async function restoreSession() {
      try {
        const [token, settings] = await Promise.all([
          AsyncStorage.getItem(AUTH_TOKEN_KEY),
          loadBiometricSettings(),
        ]);

        await refreshBiometricCapabilities();
        setHasStoredToken(Boolean(token));

        if (!token) {
          dispatch({ type: 'SET_LOADING', payload: false });
          return;
        }

        if (isBiometricEnabled(settings)) {
          const success = await loginWithBiometrics(settings);
          if (!success) {
            dispatch({ type: 'SET_LOADING', payload: false });
          }
          return;
        }

        await restoreSessionFromToken(token);
      } catch {
        await AsyncStorage.removeItem(AUTH_TOKEN_KEY);
        setHasStoredToken(false);
        dispatch({ type: 'SET_LOADING', payload: false });
      }
    }

    restoreSession();
  }, [loadBiometricSettings, loginWithBiometrics, refreshBiometricCapabilities, restoreSessionFromToken]);

  async function login(
    userNameOrEmail: string,
    password: string,
    options?: { rememberMe?: boolean }
  ) {
    const res = await api.login(userNameOrEmail, password, options?.rememberMe ?? false);
    await AsyncStorage.setItem(AUTH_TOKEN_KEY, res.token);
    setHasStoredToken(true);
    dispatch({ type: 'LOGIN', payload: { user: res.user, token: res.token } });
    return { needsProfile: res.needsProfile, userId: res.user.id };
  }

  async function signup(payload: { email: string; password: string; fullName: string; phoneNumber: string }) {
    const res = await api.signup(payload);
    await AsyncStorage.setItem(AUTH_TOKEN_KEY, res.token);
    setHasStoredToken(true);
    dispatch({ type: 'LOGIN', payload: { user: res.user, token: res.token } });
  }

  async function fillProfile(userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) {
    const res = await api.fillProfile(userId, profile);
    await AsyncStorage.setItem(AUTH_TOKEN_KEY, res.token);
    setHasStoredToken(true);
    dispatch({ type: 'LOGIN', payload: { user: res.user, token: res.token } });
  }

  function continueAsGuest() {
    dispatch({ type: 'SET_GUEST' });
  }

  async function logout() {
    try {
      await api.logout();
    } catch {}

    await AsyncStorage.removeItem(AUTH_TOKEN_KEY);
    setHasStoredToken(false);
    dispatch({ type: 'LOGOUT' });
  }

  async function updateProfile(payload: ProfilePayload) {
    const { user } = await api.updateProfile(payload);
    dispatch({ type: 'UPDATE_USER', payload: user });
  }

  const setBiometricPreference = useCallback(async (key: BiometricPreferenceKey, enabled: boolean) => {
    const token = await AsyncStorage.getItem(AUTH_TOKEN_KEY);
    if (enabled && !token && !state.token) {
      return { enabled: false, message: 'Log in first before enabling biometric sign-in.' };
    }

    const capabilities = await refreshBiometricCapabilities();
    if (enabled) {
      if (!capabilities.hasHardware) {
        return { enabled: false, message: 'This device does not support biometric authentication.' };
      }

      if (!capabilities.isEnrolled) {
        return { enabled: false, message: 'No biometric method is enrolled on this device yet.' };
      }

      if (key === 'faceIdEnabled' && !capabilities.supportsFaceId) {
        return { enabled: false, message: 'Face ID is not available on this device.' };
      }

      if (key === 'fingerprintEnabled' && !capabilities.supportsFingerprint) {
        return { enabled: false, message: 'Fingerprint authentication is not available on this device.' };
      }

      const label = key === 'faceIdEnabled' ? 'Face ID' : 'Fingerprint';
      const confirmed = await authenticateWithDevice(`Enable ${label}`);
      if (!confirmed) {
        return { enabled: false, message: `${label} setup was cancelled.` };
      }
    }

    const nextSettings = {
      ...biometricSettings,
      [key]: enabled,
    };

    await AsyncStorage.setItem(BIOMETRIC_SETTINGS_KEY, JSON.stringify(nextSettings));
    setBiometricSettings(nextSettings);

    return {
      enabled,
      message: enabled ? 'Biometric sign-in updated.' : 'Biometric sign-in disabled.',
    };
  }, [biometricSettings, refreshBiometricCapabilities, state.token]);

  const biometricLabel = isBiometricEnabled(biometricSettings)
    ? getBiometricLabel(biometricSettings, biometricCapabilities)
    : getBiometricLabel(defaultBiometricSettings, biometricCapabilities);

  const canLoginWithBiometrics = hasStoredToken &&
    isBiometricEnabled(biometricSettings) &&
    biometricCapabilities.hasHardware &&
    biometricCapabilities.isEnrolled &&
    Boolean(biometricLabel);

  const value = useMemo(() => ({
    ...state,
    login,
    signup,
    fillProfile,
    continueAsGuest,
    logout,
    updateProfile,
    biometricSettings,
    biometricCapabilities,
    biometricLabel,
    canLoginWithBiometrics,
    refreshBiometricCapabilities,
    setBiometricPreference,
    loginWithBiometrics,
  }), [
    state,
    biometricSettings,
    biometricCapabilities,
    biometricLabel,
    canLoginWithBiometrics,
    refreshBiometricCapabilities,
    setBiometricPreference,
    loginWithBiometrics,
  ]);

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within AuthProvider');
  }

  return ctx;
}
