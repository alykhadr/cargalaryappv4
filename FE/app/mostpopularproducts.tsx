import { View, StyleSheet, FlatList, TouchableOpacity } from 'react-native';
import React, { useEffect, useState } from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import ProductCard from '../components/ProductCard';
import SkeletonCard from '../components/SkeletonCard';
import HeaderWithSearch from '../components/HeaderWithSearch';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import { ALL_CATEGORY_ID, buildFilterCategories, filterCarsByCategory } from '@/utils/catalog';

const MostPopularProducts = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { dark, colors } = useTheme();
    const [selectedCategoryId, setSelectedCategoryId] = useState(ALL_CATEGORY_ID);
    const [cars, setCars] = useState<Car[]>([]);
    const [loading, setLoading] = useState(true);
    const { categories } = useCatalogCategories(cars);

    useEffect(() => {
        api.getCars({ limit: '100', sortBy: 'popular' })
            .then(res => setCars(res.cars))
            .catch(() => {})
            .finally(() => setLoading(false));
    }, []);

    const filteredProducts = filterCarsByCategory(cars, selectedCategoryId);
    const filterCategories = buildFilterCategories(categories);

    // Category item
    const renderCategoryItem = ({ item }: { item: { id: string; name: string } }) => (
        <TouchableOpacity
            style={{
                backgroundColor: selectedCategoryId === item.id ? dark ? COLORS.dark3 : COLORS.primary : "transparent",
                padding: 10,
                marginVertical: 5,
                borderColor: dark ? COLORS.dark3 : COLORS.primary,
                borderWidth: 1.3,
                borderRadius: 24,
                marginRight: 12,
            }}
            onPress={() => setSelectedCategoryId(item.id)}>
            <Text style={{
                color: selectedCategoryId === item.id ? COLORS.white : dark ? COLORS.white : COLORS.primary
            }}>{item.name}</Text>
        </TouchableOpacity>
    );

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <HeaderWithSearch
                    title="Top Deals"
                    icon={icons.search}
                    onPress={() => navigation.navigate("search")}
                />
                <ScrollView
                    style={styles.scrollView}
                    showsVerticalScrollIndicator={false}>
                    <FlatList
                        data={filterCategories}
                        keyExtractor={item => item.id}
                        showsHorizontalScrollIndicator={false}
                        horizontal
                        renderItem={renderCategoryItem}
                    />
                    <View style={{
                        backgroundColor: dark ? COLORS.dark1 : COLORS.white,
                        marginVertical: 16
                    }}>
                        {loading ? (
                            <FlatList
                                data={[1, 2, 3, 4, 5, 6]}
                                keyExtractor={item => item.toString()}
                                numColumns={2}
                                columnWrapperStyle={{ gap: 16 }}
                                scrollEnabled={false}
                                renderItem={() => <SkeletonCard />}
                            />
                        ) : filteredProducts.length === 0 ? (
                            <Text style={[styles.emptyText, {
                                color: dark ? COLORS.grayscale400 : COLORS.grayscale700
                            }]}>
                                No cars found for this category.
                            </Text>
                        ) : (
                            <FlatList
                                data={filteredProducts}
                                keyExtractor={item => item.id}
                                numColumns={2}
                                columnWrapperStyle={{ gap: 16 }}
                                showsVerticalScrollIndicator={false}
                                renderItem={({ item }) => {
                                    return (
                                        <ProductCard
                                            carId={item.id}
                                            name={item.name}
                                            image={resolveCarImage(item.imageKey)}
                                            numSolds={item.numSolds}
                                            price={item.price}
                                            rating={item.rating}
                                            onPress={() => navigation.navigate("cardetails", { carId: item.id })}
                                        />
                                    )
                                }}
                            />
                        )}
                    </View>
                </ScrollView>
            </View>
        </SafeAreaView>
    )
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
    scrollView: {
        marginBottom: 16
    },
    emptyText: {
        fontSize: 14,
        fontFamily: 'medium',
        paddingVertical: 20,
    },
})

export default MostPopularProducts
