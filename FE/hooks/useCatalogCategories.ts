import { useEffect, useState } from 'react';
import { api } from '@/services/api';
import { Car, CatalogCategory } from '@/types';
import { buildCatalogCategories } from '@/utils/catalog';

export function useCatalogCategories(fallbackCars: Car[] = []) {
  const [categories, setCategories] = useState<CatalogCategory[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;

    api.getCategories()
      .then((brands) => {
        if (cancelled) {
          return;
        }

        setCategories(buildCatalogCategories(brands, fallbackCars));
      })
      .catch(() => {
        if (cancelled) {
          return;
        }

        setCategories(buildCatalogCategories([], fallbackCars));
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  useEffect(() => {
    if (categories.length === 0 && fallbackCars.length > 0) {
      setCategories(buildCatalogCategories([], fallbackCars));
    }
  }, [categories.length, fallbackCars]);

  return { categories, loading };
}
