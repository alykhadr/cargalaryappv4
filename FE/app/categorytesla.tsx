import { View, StyleSheet, FlatList } from 'react-native';
import React, { useEffect, useState } from 'react';
import { COLORS, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import ProductCard from '../components/ProductCard';
import SkeletonCard from '../components/SkeletonCard';
import HeaderWithSearch from '../components/HeaderWithSearch';
import { NavigationProp, useNavigation } from '@react-navigation/native';

const CategoryTesla = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { dark, colors } = useTheme();
    const [cars, setCars] = useState<Car[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.getCars({ categoryId: '2' })
            .then(res => setCars(res.cars))
            .catch(() => {})
            .finally(() => setLoading(false));
    }, []);

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <HeaderWithSearch title="Tesla" icon={icons.search} onPress={() => navigation.navigate("search")} />
                <ScrollView style={styles.scrollView} showsVerticalScrollIndicator={false}>
                    <View style={{ backgroundColor: dark ? COLORS.dark1 : COLORS.white, marginVertical: 16 }}>
                        {loading ? (
                            <FlatList data={[1,2,3,4]} keyExtractor={i => i.toString()} numColumns={2}
                                columnWrapperStyle={{ gap: 16 }} scrollEnabled={false}
                                renderItem={() => <SkeletonCard />} />
                        ) : (
                            <FlatList data={cars} keyExtractor={i => i.id} numColumns={2}
                                columnWrapperStyle={{ gap: 16 }} showsVerticalScrollIndicator={false} scrollEnabled={false}
                                renderItem={({ item }) => (
                                    <ProductCard name={item.name} image={resolveCarImage(item.imageKey)}
                                        numSolds={item.numSolds} price={item.price} rating={item.rating}
                                        onPress={() => navigation.navigate("cardetails", { carId: item.id })} />
                                )} />
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
});

export default CategoryTesla;
