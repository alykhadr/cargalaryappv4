import { View, StyleSheet, FlatList, TouchableOpacity, Image } from 'react-native';
import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, icons, illustrations } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import HeaderWithSearch from '@/components/HeaderWithSearch';
import WishlistCard from '@/components/WishlistCard';
import SkeletonCard from '@/components/SkeletonCard';
import { NavigationProp, useNavigation, useFocusEffect } from '@react-navigation/native';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import { useAuth } from '@/context/AuthContext';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import { ALL_CATEGORY_ID, buildFilterCategories, filterCarsByCategory } from '@/utils/catalog';

const MyWishlist = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { dark, colors } = useTheme();
    const [selectedCategoryId, setSelectedCategoryId] = useState(ALL_CATEGORY_ID);
    const [items, setItems] = useState<Car[]>([]);
    const [wishlistLoading, setWishlistLoading] = useState(false);
    const { user, isGuest } = useAuth();
    const { categories } = useCatalogCategories(items);

    useFocusEffect(
        React.useCallback(() => {
            if (!user || isGuest) return;
            setWishlistLoading(true);
            api.getWishlist()
                .then(res => setItems(res.items))
                .catch(() => {})
                .finally(() => setWishlistLoading(false));
        }, [user, isGuest])
    );

    const handleRemove = async (carId: string) => {
        try {
            await api.removeFromWishlist(carId);
            setItems(prev => prev.filter(c => c.id !== carId));
        } catch {}
    };

    const filterCategories = buildFilterCategories(categories);

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

    if (isGuest || !user) {
        return (
            <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
                <View style={[styles.container, { backgroundColor: colors.background }]}>
                    <HeaderWithSearch
                        title="My Wishlist"
                        icon={icons.search}
                        onPress={() => navigation.navigate("search")}
                    />
                    <View style={styles.guestContainer}>
                        <Text style={[styles.guestText, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                            Please login to view your wishlist
                        </Text>
                        <TouchableOpacity
                            style={styles.loginBtn}
                            onPress={() => navigation.navigate('login')}>
                            <Text style={styles.loginBtnText}>Login</Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </SafeAreaView>
        );
    }

    const filteredItems = filterCarsByCategory(items, selectedCategoryId);

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <HeaderWithSearch
                    title="My Wishlist"
                    icon={icons.search}
                    onPress={() => navigation.navigate("search")}
                />
                <ScrollView style={styles.scrollView} showsVerticalScrollIndicator={false}>
                    <FlatList
                        data={filterCategories}
                        keyExtractor={item => item.id}
                        showsHorizontalScrollIndicator={false}
                        horizontal
                        renderItem={renderCategoryItem}
                    />
                    <View style={{ backgroundColor: dark ? COLORS.dark1 : COLORS.white, marginVertical: 16 }}>
                        {wishlistLoading ? (
                            <FlatList
                                data={[1, 2, 3, 4]}
                                keyExtractor={item => item.toString()}
                                numColumns={2}
                                columnWrapperStyle={{ gap: 16 }}
                                scrollEnabled={false}
                                renderItem={() => <SkeletonCard />}
                            />
                        ) : filteredItems.length === 0 ? (
                            <View style={styles.emptyContainer}>
                                <Image
                                    source={illustrations.empty}
                                    style={styles.emptyImage}
                                    resizeMode="contain"
                                />
                                <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                                    {items.length === 0 ? 'Your wishlist is empty' : 'No wishlist cars in this category'}
                                </Text>
                                <Text style={[styles.emptySubtitle, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
                                    {items.length === 0 ? 'Tap the heart on any car to save it here' : 'Try another category to see your saved cars'}
                                </Text>
                            </View>
                        ) : (
                            <FlatList
                                data={filteredItems}
                                keyExtractor={item => item.id}
                                numColumns={2}
                                columnWrapperStyle={{ gap: 16 }}
                                showsVerticalScrollIndicator={false}
                                scrollEnabled={false}
                                renderItem={({ item }) => (
                                    <WishlistCard
                                        name={item.name}
                                        image={resolveCarImage(item.imageKey)}
                                        numSolds={item.numSolds}
                                        rating={item.rating}
                                        price={item.price}
                                        onPress={() => navigation.navigate("cardetails", { carId: item.id })}
                                        onRemove={() => handleRemove(item.id)}
                                    />
                                )}
                            />
                        )}
                    </View>
                </ScrollView>
            </View>
        </SafeAreaView>
    )
};

const styles = StyleSheet.create({
    area: { flex: 1, backgroundColor: COLORS.white },
    container: { flex: 1, backgroundColor: COLORS.white, padding: 16 },
    scrollView: { marginVertical: 2 },
    guestContainer: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center',
    },
    guestText: {
        fontSize: 16,
        fontFamily: 'regular',
        color: COLORS.greyscale900,
        textAlign: 'center',
        marginBottom: 24,
    },
    loginBtn: {
        backgroundColor: COLORS.primary,
        paddingHorizontal: 40,
        paddingVertical: 14,
        borderRadius: 30,
    },
    loginBtnText: {
        color: COLORS.white,
        fontSize: 16,
        fontFamily: 'semiBold',
    },
    emptyContainer: {
        alignItems: 'center',
        justifyContent: 'center',
        paddingTop: 48,
        paddingBottom: 32,
    },
    emptyImage: {
        width: 160,
        height: 160,
        marginBottom: 16,
    },
    emptyTitle: {
        fontSize: 18,
        fontFamily: 'bold',
        textAlign: 'center',
        marginBottom: 8,
    },
    emptySubtitle: {
        fontSize: 14,
        fontFamily: 'regular',
        textAlign: 'center',
        paddingHorizontal: 32,
    },
})

export default MyWishlist
