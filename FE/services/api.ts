import AsyncStorage from '@react-native-async-storage/async-storage';
import { Platform } from 'react-native';
import { Car, CarQueryParams, Inquiry, ProfilePayload, User } from '../types';

const BASE_URL =
  Platform.OS === 'android'
    ? 'http://10.0.2.2:5121/api'
    : 'http://localhost:5121/api';

async function request<T>(
  path: string,
  options: { method?: string; body?: object; requiresAuth?: boolean } = {}
): Promise<T> {
  const { method = 'GET', body, requiresAuth = false } = options;
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };

  if (requiresAuth) {
    const token = await AsyncStorage.getItem('authToken');
    if (token) headers['Authorization'] = `Bearer ${token}`;
  }

  const res = await fetch(`${BASE_URL}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  const data = await res.json();
  if (!res.ok) throw new Error(data.error || `Request failed: ${res.status}`);
  return data as T;
}

export const api = {
  // Auth
  signup: (email: string, password: string) =>
    request<{ userId: string }>('/auth/signup', { method: 'POST', body: { email, password } }),

  fillProfile: (userId: string, profile: Omit<ProfilePayload, 'avatarUrl'> & { fullName: string }) =>
    request<{ token: string; user: User }>('/auth/fill-profile', {
      method: 'POST',
      body: { userId, ...profile },
    }),

  login: (email: string, password: string) =>
    request<{ token: string; user: User; needsProfile?: boolean }>('/auth/login', {
      method: 'POST',
      body: { email, password },
    }),

  logout: () =>
    request<{ success: boolean }>('/auth/logout', { method: 'POST', requiresAuth: true }),

  getMe: () =>
    request<{ user: User }>('/auth/me', { requiresAuth: true }),

  updateProfile: (payload: ProfilePayload) =>
    request<{ user: User }>('/auth/profile', { method: 'PUT', body: payload, requiresAuth: true }),

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
};
