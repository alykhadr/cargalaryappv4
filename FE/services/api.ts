import AsyncStorage from '@react-native-async-storage/async-storage';
import Constants from 'expo-constants';
import { NativeModules, Platform } from 'react-native';
import { Brand, Car, CarQueryParams, CompanyInformation, Inquiry, PrivacyPolicy, ProfilePayload, User } from '../types';

const API_BASE_URL = resolveApiBaseUrl().replace(/\/$/, '');

function resolveApiBaseUrl() {
  const envUrl = process.env.EXPO_PUBLIC_CARGALARY_API_URL?.trim();
  if (envUrl) {
    return envUrl;
  }

  const devServerHost = resolveDevServerHost();
  if (devServerHost) {
    return `http://${devServerHost}:5121/api`;
  }

  return Platform.OS === 'android'
    ? 'http://10.0.2.2:5121/api'
    : 'http://localhost:5121/api';
}

function resolveDevServerHost() {
  const manifestDebuggerHost = (Constants as any)?.manifest?.debuggerHost as string | undefined;
  const manifest2DebuggerHost = (Constants as any)?.manifest2?.extra?.expoGo?.debuggerHost as string | undefined;
  const expoHostUri = Constants.expoConfig?.hostUri;
  const scriptUrl = NativeModules.SourceCode?.scriptURL as string | undefined;

  const candidates = [
    scriptUrl,
    expoHostUri,
    manifest2DebuggerHost,
    manifestDebuggerHost,
  ];

  for (const candidate of candidates) {
    const host = extractHost(candidate);
    if (host) {
      return host;
    }
  }

  return null;
}

function extractHost(value?: string | null) {
  if (!value) {
    return null;
  }

  try {
    const normalized = value.includes('://') ? value : `http://${value}`;
    const hostname = new URL(normalized).hostname;
    if (!hostname) {
      return null;
    }

    if (hostname === 'localhost' || hostname === '127.0.0.1' || hostname === '0.0.0.0') {
      return Platform.OS === 'android' ? '10.0.2.2' : 'localhost';
    }

    return hostname;
  } catch {
    return null;
  }
}

async function request<T>(
  path: string,
  options: { method?: string; body?: object; requiresAuth?: boolean; baseUrl?: string } = {}
): Promise<T> {
  const { method = 'GET', body, requiresAuth = false, baseUrl = API_BASE_URL } = options;
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };

  if (requiresAuth) {
    const token = await AsyncStorage.getItem('authToken');
    if (token) headers['Authorization'] = `Bearer ${token}`;
  }

  const res = await fetch(`${baseUrl}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  const text = await res.text();
  const data = text ? tryParseJson(text) : null;
  if (!res.ok) {
    const errorMessage = extractErrorMessage(data, res.status);
    throw new Error(errorMessage);
  }

  return data as T;
}

function tryParseJson(text: string) {
  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

function extractErrorMessage(data: unknown, status: number): string {
  if (typeof data === 'string' && data.trim().length > 0) {
    return data;
  }

  if (data && typeof data === 'object') {
    const payload = data as {
      errors?: string[];
      messageEn?: string;
      message?: string;
      error?: string;
    };

    if (Array.isArray(payload.errors) && payload.errors.length > 0) {
      return payload.errors.join('\n');
    }

    return payload.messageEn || payload.message || payload.error || `Request failed: ${status}`;
  }

  return `Request failed: ${status}`;
}

type AuthResponse = {
  token: string;
  user: User;
  needsProfile?: boolean;
};

type RegisterPayload = {
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
};

type ForgotPasswordPayload = {
  userNameOrEmail: string;
};

export const api = {
  // Auth
  signup: ({ email, password, fullName, phoneNumber }: RegisterPayload) =>
    request<AuthResponse>('/auth/register', {
      method: 'POST',
      body: {
        email,
        userName: email,
        password,
        nameEn: fullName,
        nameAr: fullName,
        phoneNumber,
      },
    }),

  fillProfile: (userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) =>
    request<{ token: string; user: User }>('/auth/fill-profile', {
      method: 'POST',
      body: { userId, ...profile },
    }),

  login: (userName: string, password: string) =>
    request<AuthResponse>('/auth/login', {
      method: 'POST',
      body: { userName, password },
    }),

  forgotPassword: ({ userNameOrEmail }: ForgotPasswordPayload) =>
    request<{ message: string }>('/auth/forgot-password', {
      method: 'POST',
      body: { userNameOrEmail },
    }),

  logout: () =>
    request<{ success: boolean }>('/auth/logout', { method: 'POST', requiresAuth: true }),

  getMe: () =>
    request<{ user: User }>('/auth/me', { requiresAuth: true }),

  updateProfile: (payload: ProfilePayload) =>
    request<{ user: User }>('/auth/profile', { method: 'PUT', body: payload, requiresAuth: true }),

  // Categories / brands
  getCategories: () =>
    request<Brand[]>('/brand'),

  // Cars
  getCars: (params: CarQueryParams = {}) => {
    const qs = new URLSearchParams(params as Record<string, string>).toString();
    return request<{ cars: Car[]; total: number }>(`/cars${qs ? `?${qs}` : ''}`);
  },

  getCar: (id: string) =>
    request<{ car: Car }>(`/cars/${id}`),

  // Wishlist
  getWishlist: () =>
    request<{ items: Car[] }>('/wishlist', { requiresAuth: true }),

  getWishlistStatus: (carId: string) =>
    request<{ isWishlisted: boolean }>(`/wishlist/${carId}/status`, { requiresAuth: true }),

  addToWishlist: (carId: string) =>
    request<{ success: boolean }>(`/wishlist/${carId}`, { method: 'POST', requiresAuth: true }),

  removeFromWishlist: (carId: string) =>
    request<{ success: boolean }>(`/wishlist/${carId}`, { method: 'DELETE', requiresAuth: true }),

  // Inquiries
  getInquiries: () =>
    request<{ inquiries: (Inquiry & { car: Car | null })[] }>('/inquiries', { requiresAuth: true }),

  submitInquiry: (carId: string, name: string, phone: string, message: string) =>
    request<{ inquiry: Inquiry }>('/inquiries', {
      method: 'POST',
      body: { carId, name, phone, message },
      requiresAuth: true,
    }),

  // Privacy policy
  getPrivacyPolicy: () =>
    request<PrivacyPolicy>('/privacy-policy'),

  getCompanyInformation: () =>
    request<CompanyInformation>('/company-information'),
};
