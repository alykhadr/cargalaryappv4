import { View, StyleSheet, FlatList } from 'react-native';
import React, { useEffect, useState } from 'react';
import { COLORS, icons } from '@/constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '@/theme/ThemeProvider';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import ProductCard from '@/components/ProductCard';
import SkeletonCard from '@/components/SkeletonCard';
import HeaderWithSearch from '@/components/HeaderWithSearch';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import Text from '@/components/LocalizedText';
import NotFoundCard from '@/components/NotFoundCard';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';

const CategoryDetails = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const router = useRouter();
  const params = useLocalSearchParams<{ id?: string | string[]; title?: string | string[] }>();
  const { dark, colors } = useTheme();
  const [cars, setCars] = useState<Car[]>([]);
  const [loading, setLoading] = useState(true);
  const { categories } = useCatalogCategories();

  const categoryId = Array.isArray(params.id) ? params.id[0] : params.id;
  const categoryTitleParam = Array.isArray(params.title) ? params.title[0] : params.title;
  const categoryTitle = categoryTitleParam || categories.find((item) => item.id === categoryId)?.name || 'Category';

  useEffect(() => {
    if (!categoryId) {
      setCars([]);
      setLoading(false);
      return;
    }

    setLoading(true);
    api.getCars({ categoryId, limit: '100' })
      .then((res) => setCars(res.cars))
      .catch(() => setCars([]))
      .finally(() => setLoading(false));
  }, [categoryId]);

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <HeaderWithSearch
          title={categoryTitle}
          icon={icons.search}
          onPress={() => router.push('/search')}
        />
        <ScrollView style={styles.scrollView} showsVerticalScrollIndicator={false}>
          <View style={{ backgroundColor: dark ? COLORS.dark1 : COLORS.white, marginVertical: 16 }}>
            {loading ? (
              <FlatList
                data={[1, 2, 3, 4]}
                keyExtractor={(item) => item.toString()}
                numColumns={2}
                columnWrapperStyle={{ gap: 16 }}
                scrollEnabled={false}
                renderItem={() => <SkeletonCard />}
              />
            ) : cars.length === 0 ? (
              <View style={styles.emptyWrap}>
                <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                  No cars found
                </Text>
                <NotFoundCard />
              </View>
            ) : (
              <FlatList
                data={cars}
                keyExtractor={(item) => item.id}
                numColumns={2}
                columnWrapperStyle={{ gap: 16 }}
                showsVerticalScrollIndicator={false}
                scrollEnabled={false}
                renderItem={({ item }) => (
                  <ProductCard
                    carId={item.id}
                    name={item.name}
                    image={resolveCarImage(item.imageKey)}
                    numSolds={item.numSolds}
                    price={item.price}
                    rating={item.rating}
                    onPress={() => navigation.navigate('cardetails', { carId: item.id })}
                  />
                )}
              />
            )}
          </View>
        </ScrollView>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: { flex: 1, backgroundColor: COLORS.white },
  container: { flex: 1, backgroundColor: COLORS.white, padding: 16 },
  scrollView: { marginVertical: 2 },
  emptyWrap: {
    paddingTop: 12,
  },
  emptyTitle: {
    fontSize: 16,
    fontFamily: 'semiBold',
    marginBottom: 12,
  },
});

export default CategoryDetails;
