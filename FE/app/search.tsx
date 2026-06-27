import { View, StyleSheet, TouchableOpacity, Image, FlatList } from 'react-native';
import React, { useEffect, useRef, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { COLORS, SIZES, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ratings, sorts } from '../data';
import RBSheet from "react-native-raw-bottom-sheet";
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import Button from '../components/Button';
import { useTheme } from '../theme/ThemeProvider';
import MultiSlider from '@ptomasroos/react-native-multi-slider';
import { FontAwesome } from "@expo/vector-icons";
import ProductCard from '../components/ProductCard';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import NotFoundCard from '@/components/NotFoundCard';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import { ALL_CATEGORY_ID, buildFilterCategories } from '@/utils/catalog';
import SkeletonCard from '@/components/SkeletonCard';

const DEFAULT_MIN_PRICE = 0;
const DEFAULT_MAX_PRICE = 1500000;

interface SliderHandleProps {
    enabled: boolean;
    markerStyle: object;
}

const CustomSliderHandle: React.FC<SliderHandleProps> = ({ enabled, markerStyle }) => {
    return (
        <View
            style={[
                markerStyle,
                {
                    backgroundColor: enabled ? COLORS.primary : 'lightgray',
                    borderColor: 'white',
                    borderWidth: 2,
                    borderRadius: 10,
                    width: 20,
                    height: 20,
                },
            ]}
        />
    );
};

const Search = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const refRBSheet = useRef<any>(null);
    const { dark, colors } = useTheme();
    const [selectedCategoryId, setSelectedCategoryId] = useState(ALL_CATEGORY_ID);
    const [selectedSortId, setSelectedSortId] = useState("2");
    const [selectedRatingId, setSelectedRatingId] = useState<string | null>(null);
    const [priceRange, setPriceRange] = useState([DEFAULT_MIN_PRICE, DEFAULT_MAX_PRICE]);
    const [selectedTab, setSelectedTab] = useState('row');
    const [searchQuery, setSearchQuery] = useState('');
    const [filteredProducts, setFilteredProducts] = useState<Car[]>([]);
    const [resultsCount, setResultsCount] = useState(0);
    const [isSearching, setIsSearching] = useState(true);
    const { categories } = useCatalogCategories();
    const filterCategories = buildFilterCategories(categories);

    const handleSliderChange = (values: number[]) => {
        setPriceRange(values);
    };

    const handleSearch = async (
        query?: string,
        categoryId: string = selectedCategoryId,
        sortId: string = selectedSortId,
        ratingId: string | null = selectedRatingId,
        nextPriceRange: number[] = priceRange,
    ) => {
        const q = query !== undefined ? query : searchQuery;
        setIsSearching(true);
        try {
            const res = await api.getCars({
                search: q || undefined,
                categoryId: categoryId !== ALL_CATEGORY_ID ? categoryId : undefined,
                sortBy: mapSortIdToApiValue(sortId),
                minPrice: nextPriceRange[0] > DEFAULT_MIN_PRICE ? nextPriceRange[0].toString() : undefined,
                maxPrice: nextPriceRange[1] < DEFAULT_MAX_PRICE ? nextPriceRange[1].toString() : undefined,
            });
            const visibleCars = applyRatingFilter(res.cars, ratingId);
            setFilteredProducts(visibleCars);
            setResultsCount(visibleCars.length);
        } catch {
            setFilteredProducts([]);
            setResultsCount(0);
        } finally {
            setIsSearching(false);
        }
    };

    useEffect(() => {
        handleSearch();
    }, [selectedCategoryId]);
    /**
    * Render header
    */
    const renderHeader = () => {
        return (
            <View style={styles.headerContainer}>
                <View style={styles.headerLeft}>
                    <TouchableOpacity
                        onPress={() => navigation.goBack()}>
                        <Image
                            source={icons.back}
                            resizeMode='contain'
                            style={[styles.backIcon, {
                                tintColor: dark ? COLORS.white : COLORS.greyscale900
                            }]}
                        />
                    </TouchableOpacity>
                    <Text style={[styles.headerTitle, {
                        color: dark ? COLORS.white : COLORS.greyscale900
                    }]}>
                        Search
                    </Text>
                </View>
                <TouchableOpacity>
                    <Image
                        source={icons.moreCircle}
                        resizeMode='contain'
                        style={[styles.moreIcon, {
                            tintColor: dark ? COLORS.white : COLORS.greyscale900
                        }]}
                    />
                </TouchableOpacity>
            </View>
        )
    }
    /**
     * Render content
    */
    const renderContent = () => {
        const hasQuery = searchQuery.trim().length > 0;
        const resultsLabel = resultsCount === 1 ? '1 result' : `${resultsCount} results`;

        return (
            <View>
                {/* Search bar */}
                <View
                    style={[styles.searchBarContainer, {
                        backgroundColor: dark ? COLORS.dark2 : COLORS.silver
                    }]}>
                    <TouchableOpacity
                        onPress={() => handleSearch()}>
                        <Image
                            source={icons.search2}
                            resizeMode='contain'
                            style={styles.searchIcon}
                        />
                    </TouchableOpacity>
                    <TextInput
                        placeholder='Search'
                        placeholderTextColor={COLORS.gray}
                        style={[styles.searchInput, {
                            color: dark ? COLORS.white : COLORS.greyscale900
                        }]}
                        value={searchQuery}
                        onChangeText={(text) => { setSearchQuery(text); handleSearch(text); }}
                        onSubmitEditing={() => handleSearch()}
                    />
                    <TouchableOpacity
                        onPress={() => refRBSheet.current.open()}>
                        <Image
                            source={icons.filter}
                            resizeMode='contain'
                            style={[styles.filterIcon, {
                                tintColor: dark ? COLORS.white : COLORS.primary
                            }]}
                        />
                    </TouchableOpacity>
                </View>

                <View style={styles.reusltTabContainer}>
                    {
                        hasQuery ? (
                            <>
                                <Text style={[styles.tabText, {
                                    color: dark ? COLORS.secondaryWhite : COLORS.black
                                }]}>{`Result for "${searchQuery}"`}</Text>
                            </>
                        ) : (
                            <Text style={[styles.tabText, {
                                color: dark ? COLORS.secondaryWhite : COLORS.black
                            }]}>Cars</Text>
                        )
                    }
                    <View>
                        <Text style={[styles.tabText, {
                            color: dark ? COLORS.secondaryWhite : COLORS.black
                        }]}>{isSearching ? 'Searching...' : resultsLabel}</Text>
                    </View>
                </View>

                {/* Results container  */}
                <View>
                    {/* result list */}
                    <View style={{
                        backgroundColor: dark ? COLORS.dark1 : COLORS.white,
                        marginVertical: 16
                    }}>
                        {isSearching ? (
                            <FlatList
                                data={[1, 2, 3, 4]}
                                keyExtractor={(item) => item.toString()}
                                numColumns={2}
                                showsVerticalScrollIndicator={false}
                                columnWrapperStyle={{ gap: 16 }}
                                renderItem={() => <SkeletonCard />}
                            />
                        ) : resultsCount > 0 ? (
                            <>
                                <FlatList
                                    data={filteredProducts}
                                    keyExtractor={(item) => item.id}
                                    numColumns={2}
                                    showsVerticalScrollIndicator={false}
                                    columnWrapperStyle={{ gap: 16 }}
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
                            </>
                        ) : hasQuery || selectedCategoryId !== ALL_CATEGORY_ID ? (
                            <NotFoundCard />
                        ) : (
                            <Text style={[styles.emptyStateText, {
                                color: dark ? COLORS.secondaryWhite : COLORS.gray
                            }]}>
                                Start typing to search cars, or open filters to browse by category.
                            </Text>
                        )}
                    </View>
                </View>
            </View>
        )
    }

    const toggleCategory = (categoryId: string) => {
        setSelectedCategoryId(categoryId);
        handleSearch(searchQuery, categoryId, selectedSortId, selectedRatingId, priceRange);
    };

    const toggleSort = (sortId: string) => {
        setSelectedSortId(sortId);
    };

    const toggleRating = (ratingId: string) => {
        setSelectedRatingId((current) => current === ratingId ? null : ratingId);
    };

    // Category item
    const renderCategoryItem = ({ item }: { item: { id: string; name: string } }) => (
        <TouchableOpacity
            style={{
                backgroundColor: selectedCategoryId === item.id ? COLORS.primary : "transparent",
                padding: 10,
                marginVertical: 5,
                borderColor: COLORS.primary,
                borderWidth: 1.3,
                borderRadius: 24,
                marginRight: 12,
            }}
            onPress={() => toggleCategory(item.id)}>

            <Text style={{
                color: selectedCategoryId === item.id ? COLORS.white : COLORS.primary
            }}>{item.name}</Text>
        </TouchableOpacity>
    );

    // Sort item
    const renderSortItem = ({ item }: { item: { id: string; name: string } }) => (
        <TouchableOpacity
            style={{
                backgroundColor: selectedSortId === item.id ? COLORS.primary : "transparent",
                padding: 10,
                marginVertical: 5,
                borderColor: COLORS.primary,
                borderWidth: 1.3,
                borderRadius: 24,
                marginRight: 12,
            }}
            onPress={() => toggleSort(item.id)}>

            <Text style={{
                color: selectedSortId === item.id ? COLORS.white : COLORS.primary
            }}>{item.name}</Text>
        </TouchableOpacity>
    );

    const renderRatingItem = ({ item }: { item: { id: string; title: string } }) => (
        <TouchableOpacity
            style={{
                backgroundColor: selectedRatingId === item.id ? COLORS.primary : "transparent",
                paddingHorizontal: 16,
                paddingVertical: 6,
                marginVertical: 5,
                borderColor: COLORS.primary,
                borderWidth: 1.3,
                borderRadius: 24,
                marginRight: 12,
                flexDirection: "row",
                alignItems: "center",
            }}
            onPress={() => toggleRating(item.id)}>
            <View style={{ marginRight: 6 }}>
                <FontAwesome name="star" size={14} color={selectedRatingId === item.id ? COLORS.white : COLORS.primary} />
            </View>
            <Text style={{
                color: selectedRatingId === item.id ? COLORS.white : COLORS.primary
            }}>{item.title}</Text>
        </TouchableOpacity>
    );

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                {renderHeader()}
                <View>
                    {renderContent()}
                </View>
                <RBSheet
                    ref={refRBSheet}
                    closeOnPressMask={true}
                    height={580}
                    customStyles={{
                        wrapper: {
                            backgroundColor: "rgba(0,0,0,0.5)",
                        },
                        draggableIcon: {
                            backgroundColor: dark ? COLORS.dark3 : "#000",
                        },
                        container: {
                            borderTopRightRadius: 32,
                            borderTopLeftRadius: 32,
                            height: 580,
                            backgroundColor: dark ? COLORS.dark2 : COLORS.white,
                            alignItems: "center",
                        }
                    }}>
                    <Text style={[styles.bottomTitle, {
                        color: dark ? COLORS.white : COLORS.greyscale900
                    }]}>Filter</Text>
                    <View style={styles.separateLine} />
                    <View style={{ width: SIZES.width - 32 }}>
                        <Text style={[styles.sheetTitle, {
                            color: dark ? COLORS.white : COLORS.greyscale900
                        }]}>Category</Text>
                        <FlatList
                            data={filterCategories}
                            keyExtractor={item => item.id}
                            showsHorizontalScrollIndicator={false}
                            horizontal
                            renderItem={renderCategoryItem}
                        />
                        <Text style={[styles.sheetTitle, {
                            color: dark ? COLORS.white : COLORS.greyscale900
                        }]}>Filter</Text>
                        <MultiSlider
                            values={priceRange}
                            sliderLength={SIZES.width - 32}
                            onValuesChange={handleSliderChange}
                            min={DEFAULT_MIN_PRICE}
                            max={DEFAULT_MAX_PRICE}
                            step={5000}
                            allowOverlap={false}
                            snapped
                            minMarkerOverlapDistance={40}
                            customMarker={CustomSliderHandle}
                            selectedStyle={{ backgroundColor: COLORS.primary }}
                            unselectedStyle={{ backgroundColor: 'lightgray' }}
                            containerStyle={{ height: 40 }}
                            trackStyle={{ height: 3 }}
                        />
                        <Text style={[styles.priceRangeText, {
                            color: dark ? COLORS.grayscale100 : COLORS.grayscale700
                        }]}>
                            {`SAR ${priceRange[0].toLocaleString()} - SAR ${priceRange[1].toLocaleString()}`}
                        </Text>
                        <Text style={[styles.sheetTitle, {
                            color: dark ? COLORS.white : COLORS.greyscale900
                        }]}>Sort by</Text>
                        <FlatList
                            data={sorts}
                            keyExtractor={item => item.id}
                            showsHorizontalScrollIndicator={false}
                            horizontal
                            renderItem={renderSortItem}
                        />
                        <Text style={[styles.sheetTitle, {
                            color: dark ? COLORS.white : COLORS.greyscale900
                        }]}>Rating</Text>
                        <FlatList
                            data={ratings}
                            keyExtractor={item => item.id}
                            showsHorizontalScrollIndicator={false}
                            horizontal
                            renderItem={renderRatingItem}
                        />
                    </View>

                    <View style={styles.separateLine} />

                    <View style={styles.bottomContainer}>
                        <Button
                            title="Reset"
                            style={{
                                width: (SIZES.width - 32) / 2 - 8,
                                backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
                                borderRadius: 32,
                                borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary
                            }}
                            textColor={dark ? COLORS.white : COLORS.primary}
                            onPress={() => refRBSheet.current.close()}
                        />
                        <Button
                            title="Filter"
                            filled
                            style={styles.logoutButton}
                            onPress={() => {
                                handleSearch(searchQuery, selectedCategoryId, selectedSortId, selectedRatingId, priceRange);
                                refRBSheet.current.close();
                            }}
                        />
                    </View>
                </RBSheet>
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
    headerContainer: {
        flexDirection: "row",
        width: SIZES.width - 32,
        justifyContent: "space-between",
        marginBottom: 16
    },
    headerLeft: {
        flexDirection: "row",
        alignItems: "center"
    },
    backIcon: {
        height: 24,
        width: 24,
        tintColor: COLORS.black
    },
    headerTitle: {
        fontSize: 20,
        fontFamily: 'bold',
        color: COLORS.black,
        marginLeft: 16
    },
    moreIcon: {
        width: 24,
        height: 24,
        tintColor: COLORS.black
    },
    searchBarContainer: {
        width: SIZES.width - 32,
        backgroundColor: COLORS.secondaryWhite,
        padding: 16,
        borderRadius: 12,
        height: 52,
        marginBottom: 16,
        flexDirection: "row",
        alignItems: "center"
    },
    searchIcon: {
        height: 24,
        width: 24,
        tintColor: COLORS.gray
    },
    searchInput: {
        flex: 1,
        fontSize: 16,
        fontFamily: "regular",
        marginHorizontal: 8
    },
    filterIcon: {
        width: 24,
        height: 24,
        tintColor: COLORS.primary
    },
    tabContainer: {
        flexDirection: "row",
        alignItems: "center",
        width: SIZES.width - 32,
        justifyContent: "space-between"
    },
    tabBtn: {
        width: (SIZES.width - 32) / 2 - 6,
        height: 42,
        justifyContent: "center",
        alignItems: "center",
        borderWidth: 1.4,
        borderColor: COLORS.primary,
        borderRadius: 32
    },
    selectedTab: {
        width: (SIZES.width - 32) / 2 - 6,
        height: 42,
        backgroundColor: COLORS.primary,
        justifyContent: "center",
        alignItems: "center",
        borderWidth: 1.4,
        borderColor: COLORS.primary,
        borderRadius: 32
    },
    tabBtnText: {
        fontSize: 16,
        fontFamily: "semiBold",
        color: COLORS.primary,
        textAlign: "center"
    },
    selectedTabText: {
        fontSize: 16,
        fontFamily: "semiBold",
        color: COLORS.white,
        textAlign: "center"
    },
    resultContainer: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        width: SIZES.width - 32,
        marginVertical: 16,
    },
    subtitle: {
        fontSize: 18,
        fontFamily: "bold",
        color: COLORS.black,
    },
    subResult: {
        fontSize: 14,
        fontFamily: "semiBold",
        color: COLORS.primary
    },
    resultLeftView: {
        flexDirection: "row"
    },
    bottomContainer: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        marginVertical: 12,
        paddingHorizontal: 16,
        width: SIZES.width
    },
    cancelButton: {
        width: (SIZES.width - 32) / 2 - 8,
        backgroundColor: COLORS.tansparentPrimary,
        borderRadius: 32
    },
    logoutButton: {
        width: (SIZES.width - 32) / 2 - 8,
        backgroundColor: COLORS.primary,
        borderRadius: 32
    },
    bottomTitle: {
        fontSize: 24,
        fontFamily: "semiBold",
        color: COLORS.black,
        textAlign: "center",
        marginTop: 12
    },
    separateLine: {
        height: .4,
        width: SIZES.width - 32,
        backgroundColor: COLORS.greyscale300,
        marginVertical: 12
    },
    sheetTitle: {
        fontSize: 18,
        fontFamily: "semiBold",
        color: COLORS.black,
        marginVertical: 12
    },
    priceRangeText: {
        fontSize: 14,
        fontFamily: "medium",
        marginTop: 4,
    },
    reusltTabContainer: {
        flexDirection: "row",
        alignItems: "center",
        width: SIZES.width - 32,
        justifyContent: "space-between"
    },
    viewDashboard: {
        flexDirection: "row",
        alignItems: "center",
        width: 36,
        justifyContent: "space-between"
    },
    dashboardIcon: {
        width: 16,
        height: 16,
        tintColor: COLORS.primary
    },
    tabText: {
        fontSize: 20,
        fontFamily: "semiBold",
        color: COLORS.black
    },
    emptyStateText: {
        fontSize: 15,
        fontFamily: "medium",
        color: COLORS.gray,
        textAlign: "center",
        lineHeight: 22,
        paddingHorizontal: 16,
        paddingVertical: 24,
    }
})

export default Search

function mapSortIdToApiValue(sortId: string) {
    switch (sortId) {
        case "1":
            return "popular";
        case "2":
            return "recent";
        case "3":
            return "price_desc";
        case "4":
            return "price_asc";
        case "5":
            return "rating";
        default:
            return undefined;
    }
}

function applyRatingFilter(cars: Car[], ratingId: string | null) {
    if (!ratingId) {
        return cars;
    }

    const selectedRating = ratings.find((item) => item.id === ratingId);
    const minimumRating = selectedRating ? Number(selectedRating.title) : 0;

    if (!minimumRating) {
        return cars;
    }

    return cars.filter((car) => car.rating >= minimumRating);
}
