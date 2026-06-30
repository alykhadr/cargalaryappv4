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

export interface CatalogCategory {
  id: string;
  name: string;
  imageUrl?: string | null;
  brandLogoKey?: string;
}

export interface Brand {
  id: number;
  nameAr?: string | null;
  nameEn?: string | null;
  imageUrl?: string | null;
  createdBy?: string | null;
}

export interface Offer {
  id: number;
  offerImageUrl?: string | null;
  offerNameAr?: string | null;
  offerNameEn?: string | null;
  descriptionAr?: string | null;
  descriptionEn?: string | null;
  expiredAt?: string | null;
  isPercentage: boolean;
  percentageValue?: number | null;
  isAvailable: boolean;
}

export interface Car {
  id: string;
  name: string;
  model: string;
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
  year: number;
  mileage: number;
  fuelType: string;
  transmission: string;
  drivetrain: string;
  vehicleClass: string;
  seatingCapacity: number;
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

export interface CompanyInformation {
  id: number;
  companyNameAr?: string | null;
  companyNameEn?: string | null;
  crNumber?: string | null;
  vatRegistrationNumber?: string | null;
  logoUrl?: string | null;
  mobileNo?: string | null;
  telNo?: string | null;
  email?: string | null;
  aboutUsAr?: string | null;
  aboutUsEn?: string | null;
  ourMissionAr?: string | null;
  ourMissionEn?: string | null;
  ourGoalsAr?: string | null;
  ourGoalsEn?: string | null;
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
