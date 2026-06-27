import { View, StyleSheet, FlatList } from 'react-native';
import React from 'react';
import { COLORS } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import Category from '../components/Category';
import { useTheme } from '../theme/ThemeProvider';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import { getCategoryPalette } from '@/utils/catalog';
import { resolveCategoryIcon } from '@/utils/imageResolver';
import { useRouter } from 'expo-router';
import { SIZES } from '@/constants';
import { CatalogCategory } from '@/types';

const Categories = () => {
  const router = useRouter();
  const { colors, dark } = useTheme();
  const { categories, loading } = useCatalogCategories();
  const placeholders: CatalogCategory[] = Array.from({ length: 8 }, (_, index) => ({
    id: `placeholder-${index}`,
    name: '',
  }));

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Categories" />
        <FlatList
          data={loading ? placeholders : categories}
          keyExtractor={(item) => item.id}
          numColumns={4}
          columnWrapperStyle={styles.row}
          contentContainerStyle={styles.listContent}
          renderItem={({ item }) => {
            if (loading) {
              return (
                <View style={styles.placeholderItem}>
                  <View style={[styles.placeholderIcon, { backgroundColor: dark ? COLORS.dark3 : '#EEF1F6' }]} />
                  <View style={[styles.placeholderLabel, { backgroundColor: dark ? COLORS.dark3 : '#EEF1F6' }]} />
                </View>
              );
            }

            const palette = getCategoryPalette(item.id);
            return (
              <Category
                name={item.name}
                icon={resolveCategoryIcon(item.imageUrl, item.brandLogoKey)}
                preserveIconColor={Boolean(item.imageUrl || item.brandLogoKey)}
                iconColor={palette.iconColor}
                backgroundColor={palette.backgroundColor}
                onPress={() => router.push({ pathname: '/category/[id]', params: { id: item.id, title: item.name } })}
              />
            );
          }}
        />
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: {
    flex: 1,
    backgroundColor: COLORS.white
  },
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
    padding: 16
  },
  listContent: {
    paddingTop: 22,
    paddingBottom: 32,
  },
  row: {
    justifyContent: 'flex-start',
  },
  placeholderItem: {
    width: (SIZES.width - 32) / 4,
    alignItems: 'center',
    marginBottom: 12,
  },
  placeholderIcon: {
    width: 54,
    height: 54,
    borderRadius: 999,
    marginBottom: 8,
  },
  placeholderLabel: {
    width: 54,
    height: 12,
    borderRadius: 999,
  },
});

export default Categories
