export interface User {
  id: string;
  email: string;
  fullName?: string;
  nickname?: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  country?: string;
  avatarUrl?: string;
  profileComplete?: boolean;
}

export interface Car {
  id: string;
  name: string;
  imageKey: string;
  sliderImageKeys: string[];
  price: string;
  numReviews: number;
  rating: number;
  numSolds: number;
  categoryId: string;
  brand: string;
  brandLogoKey: string;
  description: string;
  colors: string[];
}

export interface Inquiry {
  id: string;
  userId: string;
  carId: string;
  name: string;
  phone: string;
  message: string;
  createdAt: string;
}

export interface PrivacyPolicy {
  id: number;
  privacyPolicyAr?: string | null;
  privacyPolicyEn?: string | null;
  isAvailable: boolean;
}

export interface ProfilePayload {
  fullName?: string;
  nickname?: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  country?: string;
  avatarUrl?: string;
}

export interface CarQueryParams {
  search?: string;
  categoryId?: string;
  sortBy?: string;
  minPrice?: string;
  maxPrice?: string;
  limit?: string;
  offset?: string;
}
