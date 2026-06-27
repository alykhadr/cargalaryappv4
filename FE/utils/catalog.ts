import { Brand, Car, CatalogCategory } from '@/types';

export const ALL_CATEGORY_ID = 'all';

export function buildCatalogCategories(brands: Brand[], cars: Car[] = []): CatalogCategory[] {
  const brandCategories: CatalogCategory[] = [];
  for (const brand of brands) {
    const name = pickFirstValue(brand.nameEn, brand.nameAr, `Brand ${brand.id}`);
    if (!name) {
      continue;
    }

    brandCategories.push({
      id: brand.id.toString(),
      name,
      imageUrl: brand.imageUrl ?? null,
      brandLogoKey: toBrandLogoKey(name),
    });
  }

  if (brandCategories.length > 0) {
    return brandCategories;
  }

  const categoryMap = new Map<string, CatalogCategory>();
  for (const car of cars) {
    if (!car.categoryId || !car.brand) {
      continue;
    }

    if (categoryMap.has(car.categoryId)) {
      continue;
    }

    categoryMap.set(car.categoryId, {
      id: car.categoryId,
      name: car.brand,
      brandLogoKey: car.brandLogoKey || toBrandLogoKey(car.brand),
    });
  }

  return Array.from(categoryMap.values());
}

export function buildFilterCategories(categories: CatalogCategory[]): CatalogCategory[] {
  return [{ id: ALL_CATEGORY_ID, name: 'All' }, ...categories];
}

export function filterCarsByCategory(cars: Car[], selectedCategoryId: string): Car[] {
  if (!selectedCategoryId || selectedCategoryId === ALL_CATEGORY_ID) {
    return cars;
  }

  return cars.filter((car) => car.categoryId === selectedCategoryId);
}

export function getCategoryPalette(seed: string) {
  const palettes = [
    { backgroundColor: '#FCE7EA', iconColor: '#D7263D' },
    { backgroundColor: '#E3F0FF', iconColor: '#1A73E8' },
    { backgroundColor: '#E9F7EF', iconColor: '#2E8B57' },
    { backgroundColor: '#FFF2D8', iconColor: '#C77600' },
    { backgroundColor: '#F0ECFF', iconColor: '#5B3CC4' },
    { backgroundColor: '#EAF7F6', iconColor: '#157A6E' },
  ];

  let hash = 0;
  for (let i = 0; i < seed.length; i += 1) {
    hash = (hash * 31 + seed.charCodeAt(i)) >>> 0;
  }

  return palettes[hash % palettes.length];
}

export function toBrandLogoKey(value: string) {
  return value.trim().toLowerCase().replace(/\s+/g, '-');
}

function pickFirstValue(...values: Array<string | null | undefined>) {
  return values.find((value) => typeof value === 'string' && value.trim().length > 0)?.trim() ?? '';
}
