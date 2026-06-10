import AsyncStorage from '@react-native-async-storage/async-storage';
import React, { createContext, useContext, useEffect, useReducer } from 'react';
import { api } from '../services/api';
import { ProfilePayload, User } from '../types';

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

interface AuthContextValue extends AuthState {
  login: (email: string, password: string) => Promise<{ needsProfile?: boolean; userId?: string }>;
  signup: (email: string, password: string) => Promise<{ userId: string }>;
  fillProfile: (userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) => Promise<void>;
  continueAsGuest: () => void;
  logout: () => Promise<void>;
  updateProfile: (payload: ProfilePayload) => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(reducer, initialState);

  useEffect(() => {
    async function restoreSession() {
      try {
        const token = await AsyncStorage.getItem('authToken');
        if (token) {
          const { user } = await api.getMe();
          dispatch({ type: 'LOGIN', payload: { user, token } });
        } else {
          dispatch({ type: 'SET_LOADING', payload: false });
        }
      } catch {
        await AsyncStorage.removeItem('authToken');
        dispatch({ type: 'SET_LOADING', payload: false });
      }
    }
    restoreSession();
  }, []);

  async function login(email: string, password: string) {
    const res = await api.login(email, password);
    await AsyncStorage.setItem('authToken', res.token);
    dispatch({ type: 'LOGIN', payload: { user: res.user, token: res.token } });
    return { needsProfile: res.needsProfile, userId: res.user.id };
  }

  async function signup(email: string, password: string) {
    return api.signup(email, password);
  }

  async function fillProfile(userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) {
    const res = await api.fillProfile(userId, profile);
    await AsyncStorage.setItem('authToken', res.token);
    dispatch({ type: 'LOGIN', payload: { user: res.user, token: res.token } });
  }

  function continueAsGuest() {
    dispatch({ type: 'SET_GUEST' });
  }

  async function logout() {
    try { await api.logout(); } catch {}
    await AsyncStorage.removeItem('authToken');
    dispatch({ type: 'LOGOUT' });
  }

  async function updateProfile(payload: ProfilePayload) {
    const { user } = await api.updateProfile(payload);
    dispatch({ type: 'UPDATE_USER', payload: user });
  }

  return (
    <AuthContext.Provider value={{ ...state, login, signup, fillProfile, continueAsGuest, logout, updateProfile }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}
