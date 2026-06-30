import { View, StyleSheet, TouchableOpacity, Image, FlatList, ScrollView } from 'react-native';
import React, { startTransition, useDeferredValue, useEffect, useRef, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { COLORS, SIZES, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ratings, sorts } from '../data';
import RBSheet from 'react-native-raw-bottom-sheet';
import { api } from '@/services/api';
import { Car } from '@/types';
import Button from '../components/Button';
import { useTheme } from '../theme/ThemeProvider';
import MultiSlider from '@ptomasroos/react-native-multi-slider';
import { FontAwesome } from '@expo/vector-icons';
import ProductCard from '../components/ProductCard';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import NotFoundCard from '@/components/NotFoundCard';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import { ALL_CATEGORY_ID, buildFilterCategories } from '@/utils/catalog';
import SkeletonCard from '@/components/SkeletonCard';
import { useLocalSearchParams } from 'expo-router';
import { resolveCarImage } from '@/utils/imageResolver';

const DEFAULT_MIN_PRICE = 0;
const DEFAULT_MAX_PRICE = 1500000;
const DEFAULT_SORT_ID = '2';
const SEARCH_DEBOUNCE_MS = 260;
const DEFAULT_BROWSE_LIMIT = '20';
const FILTERED_RESULTS_LIMIT = '100';

const YEAR_PRESETS = [
  { id: '2024_plus', label: '2024+', predicate: (car: Car) => car.year >= 2024 },
  { id: '2021_plus', label: '2021+', predicate: (car: Car) => car.year >= 2021 },
  { id: '2018_plus', label: '2018+', predicate: (car: Car) => car.year >= 2018 },
  { id: 'classic', label: 'Before 2018', predicate: (car: Car) => car.year < 2018 },
];

const MILEAGE_PRESETS = [
  { id: 'under_10k', label: '< 10k km', predicate: (car: Car) => car.mileage < 10000 },
  { id: 'under_50k', label: '< 50k km', predicate: (car: Car) => car.mileage < 50000 },
  { id: 'under_100k', label: '< 100k km', predicate: (car: Car) => car.mileage < 100000 },
  { id: '100k_plus', label: '100k+ km', predicate: (car: Car) => car.mileage >= 100000 },
];

const QUICK_PRICE_PRESETS = [
  { id: 'budget', label: 'Under 100k', values: [DEFAULT_MIN_PRICE, 100000] as [number, number] },
  { id: 'mid', label: '100k - 300k', values: [100000, 300000] as [number, number] },
  { id: 'premium', label: '300k+', values: [300000, DEFAULT_MAX_PRICE] as [number, number] },
];

interface SliderHandleProps {
  enabled: boolean;
  markerStyle: object;
}

interface FilterChipProps {
  label: string;
  selected: boolean;
  onPress: () => void;
  dark: boolean;
  icon?: React.ReactNode;
}

interface ActiveFilterProps {
  key: string;
  label: string;
  onClear: () => void;
}

interface SearchRequestState {
  searchQuery: string;
  selectedCategoryId: string;
  selectedSortId: string;
  selectedRatingId: string | null;
  selectedTransmission: string | null;
  selectedFuelType: string | null;
  selectedYearRange: string | null;
  selectedMileageBand: string | null;
  priceRange: number[];
}

const CustomSliderHandle: React.FC<SliderHandleProps> = ({ enabled, markerStyle }) => (
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

const FilterChip: React.FC<FilterChipProps> = ({ label, selected, onPress, dark, icon }) => (
  <TouchableOpacity
    activeOpacity={0.86}
    onPress={onPress}
    style={[
      styles.filterChip,
      selected
        ? styles.filterChipSelected
        : {
            backgroundColor: dark ? COLORS.dark2 : '#FFFFFF',
            borderColor: dark ? COLORS.dark3 : COLORS.primary,
          },
    ]}
  >
    {icon ? <View style={styles.filterChipIconWrap}>{icon}</View> : null}
    <Text
      style={[
        styles.filterChipText,
        { color: selected ? COLORS.white : dark ? COLORS.white : COLORS.primary },
      ]}
    >
      {label}
    </Text>
  </TouchableOpacity>
);

function isBrowseFeedState(state: SearchRequestState) {
  return (
    state.searchQuery.trim().length === 0 &&
    state.selectedCategoryId === ALL_CATEGORY_ID &&
    state.selectedSortId === DEFAULT_SORT_ID &&
    state.selectedRatingId === null &&
    state.selectedTransmission === null &&
    state.selectedFuelType === null &&
    state.selectedYearRange === null &&
    state.selectedMileageBand === null &&
    state.priceRange[0] <= DEFAULT_MIN_PRICE &&
    state.priceRange[1] >= DEFAULT_MAX_PRICE
  );
}

const Search = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const params = useLocalSearchParams<{
    q?: string | string[];
    categoryId?: string | string[];
    title?: string | string[];
    maxPrice?: string | string[];
    minPrice?: string | string[];
    sortId?: string | string[];
    ratingId?: string | string[];
    transmission?: string | string[];
    fuelType?: string | string[];
    yearRange?: string | string[];
    mileageBand?: string | string[];
  }>();
  const refRBSheet = useRef<any>(null);
  const hasBootstrappedRef = useRef(false);
  const { dark, colors } = useTheme();
  const { categories } = useCatalogCategories();
  const filterCategories = buildFilterCategories(categories);

  const initialQuery = normalizeStringParam(params.q) ?? '';
  const initialCategoryId = normalizeStringParam(params.categoryId) ?? ALL_CATEGORY_ID;
  const initialSortId = normalizeStringParam(params.sortId) ?? DEFAULT_SORT_ID;
  const initialRatingId = normalizeStringParam(params.ratingId);
  const initialTransmission = normalizeStringParam(params.transmission);
  const initialFuelType = normalizeStringParam(params.fuelType);
  const initialYearRange = normalizeStringParam(params.yearRange);
  const initialMileageBand = normalizeStringParam(params.mileageBand);
  const initialMinPrice = parseNumberParam(params.minPrice) ?? DEFAULT_MIN_PRICE;
  const initialMaxPrice = parseNumberParam(params.maxPrice) ?? DEFAULT_MAX_PRICE;

  const [selectedCategoryId, setSelectedCategoryId] = useState(initialCategoryId);
  const [selectedSortId, setSelectedSortId] = useState(initialSortId);
  const [selectedRatingId, setSelectedRatingId] = useState(initialRatingId === '1' ? null : initialRatingId);
  const [selectedTransmission, setSelectedTransmission] = useState(initialTransmission);
  const [selectedFuelType, setSelectedFuelType] = useState(initialFuelType);
  const [selectedYearRange, setSelectedYearRange] = useState(initialYearRange);
  const [selectedMileageBand, setSelectedMileageBand] = useState(initialMileageBand);
  const [priceRange, setPriceRange] = useState([initialMinPrice, initialMaxPrice]);
  const [searchQuery, setSearchQuery] = useState(initialQuery);
  const deferredSearchQuery = useDeferredValue(searchQuery.trim());

  const [cars, setCars] = useState<Car[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<Car[]>([]);
  const [resultsCount, setResultsCount] = useState(0);
  const [isSearching, setIsSearching] = useState(true);

  const routeDrivenState: SearchFilterState = {
    searchQuery: initialQuery,
    selectedCategoryId: initialCategoryId,
    selectedSortId: initialSortId,
    selectedRatingId: initialRatingId === '1' ? null : initialRatingId,
    selectedTransmission: initialTransmission,
    selectedFuelType: initialFuelType,
    selectedYearRange: initialYearRange,
    selectedMileageBand: initialMileageBand,
    priceRange: [initialMinPrice, initialMaxPrice],
  };

  const filterCount = [
    selectedCategoryId !== ALL_CATEGORY_ID,
    selectedRatingId !== null,
    selectedTransmission !== null,
    selectedFuelType !== null,
    selectedYearRange !== null,
    selectedMileageBand !== null,
    priceRange[0] > DEFAULT_MIN_PRICE || priceRange[1] < DEFAULT_MAX_PRICE,
    selectedSortId !== DEFAULT_SORT_ID,
  ].filter(Boolean).length;

  const titleParam = normalizeStringParam(params.title);
  const categoryTitle = titleParam || filterCategories.find((item) => item.id === selectedCategoryId)?.name;

  const fuelOptions = getUniqueOptions(cars, 'fuelType');
  const transmissionOptions = getUniqueOptions(cars, 'transmission');

  const activeFilters: ActiveFilterProps[] = [];
  if (selectedCategoryId !== ALL_CATEGORY_ID && categoryTitle) {
    activeFilters.push({
      key: 'category',
      label: categoryTitle,
      onClear: () => setSelectedCategoryId(ALL_CATEGORY_ID),
    });
  }
  if (selectedFuelType) {
    activeFilters.push({
      key: 'fuel',
      label: selectedFuelType,
      onClear: () => setSelectedFuelType(null),
    });
  }
  if (selectedTransmission) {
    activeFilters.push({
      key: 'transmission',
      label: selectedTransmission,
      onClear: () => setSelectedTransmission(null),
    });
  }
  if (selectedYearRange) {
    activeFilters.push({
      key: 'year',
      label: YEAR_PRESETS.find((item) => item.id === selectedYearRange)?.label ?? selectedYearRange,
      onClear: () => setSelectedYearRange(null),
    });
  }
  if (selectedMileageBand) {
    activeFilters.push({
      key: 'mileage',
      label: MILEAGE_PRESETS.find((item) => item.id === selectedMileageBand)?.label ?? selectedMileageBand,
      onClear: () => setSelectedMileageBand(null),
    });
  }
  if (selectedRatingId) {
    activeFilters.push({
      key: 'rating',
      label: `${ratings.find((item) => item.id === selectedRatingId)?.title ?? '0'}+ stars`,
      onClear: () => setSelectedRatingId(null),
    });
  }
  if (priceRange[0] > DEFAULT_MIN_PRICE || priceRange[1] < DEFAULT_MAX_PRICE) {
    activeFilters.push({
      key: 'price',
      label: `SAR ${priceRange[0].toLocaleString()} - ${priceRange[1].toLocaleString()}`,
      onClear: () => setPriceRange([DEFAULT_MIN_PRICE, DEFAULT_MAX_PRICE]),
    });
  }

  async function handleSearch(overrides?: Partial<SearchFilterState>) {
    const nextQuery = overrides?.searchQuery ?? deferredSearchQuery;
    const nextCategoryId = overrides?.selectedCategoryId ?? selectedCategoryId;
    const nextSortId = overrides?.selectedSortId ?? selectedSortId;
    const nextRatingId = overrides?.selectedRatingId ?? selectedRatingId;
    const nextTransmission = overrides?.selectedTransmission ?? selectedTransmission;
    const nextFuelType = overrides?.selectedFuelType ?? selectedFuelType;
    const nextYearRange = overrides?.selectedYearRange ?? selectedYearRange;
    const nextMileageBand = overrides?.selectedMileageBand ?? selectedMileageBand;
    const nextPriceRange = overrides?.priceRange ?? priceRange;
    const requestState: SearchRequestState = {
      searchQuery: nextQuery,
      selectedCategoryId: nextCategoryId,
      selectedSortId: nextSortId,
      selectedRatingId: nextRatingId,
      selectedTransmission: nextTransmission,
      selectedFuelType: nextFuelType,
      selectedYearRange: nextYearRange,
      selectedMileageBand: nextMileageBand,
      priceRange: nextPriceRange,
    };
    const browseFeedMode = isBrowseFeedState(requestState);

    setIsSearching(true);
    try {
      const res = await api.getCars({
        search: nextQuery || undefined,
        categoryId: nextCategoryId !== ALL_CATEGORY_ID ? nextCategoryId : undefined,
        sortBy: browseFeedMode ? undefined : mapSortIdToApiValue(nextSortId),
        minPrice: nextPriceRange[0] > DEFAULT_MIN_PRICE ? nextPriceRange[0].toString() : undefined,
        maxPrice: nextPriceRange[1] < DEFAULT_MAX_PRICE ? nextPriceRange[1].toString() : undefined,
        limit: browseFeedMode ? DEFAULT_BROWSE_LIMIT : FILTERED_RESULTS_LIMIT,
      });

      const visibleCars = applyClientSideFilters(res.cars, {
        ratingId: nextRatingId,
        transmission: nextTransmission,
        fuelType: nextFuelType,
        yearRange: nextYearRange,
        mileageBand: nextMileageBand,
      });

      startTransition(() => {
        setCars(res.cars);
        setFilteredProducts(visibleCars);
        setResultsCount(visibleCars.length);
      });
    } catch {
      startTransition(() => {
        setCars([]);
        setFilteredProducts([]);
        setResultsCount(0);
      });
    } finally {
      setIsSearching(false);
    }
  }

  useEffect(() => {
    setSearchQuery(routeDrivenState.searchQuery);
    setSelectedCategoryId(routeDrivenState.selectedCategoryId);
    setSelectedSortId(routeDrivenState.selectedSortId);
    setSelectedRatingId(routeDrivenState.selectedRatingId);
    setSelectedTransmission(routeDrivenState.selectedTransmission);
    setSelectedFuelType(routeDrivenState.selectedFuelType);
    setSelectedYearRange(routeDrivenState.selectedYearRange);
    setSelectedMileageBand(routeDrivenState.selectedMileageBand);
    setPriceRange(routeDrivenState.priceRange);

    handleSearch(routeDrivenState);
    hasBootstrappedRef.current = true;
  }, [
    initialQuery,
    initialCategoryId,
    initialSortId,
    initialRatingId,
    initialTransmission,
    initialFuelType,
    initialYearRange,
    initialMileageBand,
    initialMinPrice,
    initialMaxPrice,
  ]);

  useEffect(() => {
    if (!hasBootstrappedRef.current) {
      return;
    }

    const timer = setTimeout(() => {
      handleSearch({
        searchQuery: deferredSearchQuery,
      });
    }, SEARCH_DEBOUNCE_MS);

    return () => clearTimeout(timer);
  }, [deferredSearchQuery]);

  const hasQuery = searchQuery.trim().length > 0;
  const browseFeedMode = !hasQuery && filterCount === 0;
  const resultsLabel = resultsCount === 1 ? '1 result' : `${resultsCount} results`;

  const applyFilters = () => {
    handleSearch({
      searchQuery: searchQuery.trim(),
      selectedCategoryId,
      selectedSortId,
      selectedRatingId,
      selectedTransmission,
      selectedFuelType,
      selectedYearRange,
      selectedMileageBand,
      priceRange,
    });
    refRBSheet.current?.close();
  };

  const resetFilters = () => {
    const nextPriceRange: [number, number] = [DEFAULT_MIN_PRICE, DEFAULT_MAX_PRICE];
    setSelectedCategoryId(ALL_CATEGORY_ID);
    setSelectedSortId(DEFAULT_SORT_ID);
    setSelectedRatingId(null);
    setSelectedTransmission(null);
    setSelectedFuelType(null);
    setSelectedYearRange(null);
    setSelectedMileageBand(null);
    setPriceRange(nextPriceRange);
    handleSearch({
      searchQuery: searchQuery.trim(),
      selectedCategoryId: ALL_CATEGORY_ID,
      selectedSortId: DEFAULT_SORT_ID,
      selectedRatingId: null,
      selectedTransmission: null,
      selectedFuelType: null,
      selectedYearRange: null,
      selectedMileageBand: null,
      priceRange: nextPriceRange,
    });
    refRBSheet.current?.close();
  };

  const renderHeader = () => (
    <View style={styles.headerContainer}>
      <View style={styles.headerLeft}>
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Image
            source={icons.back}
            resizeMode="contain"
            style={[styles.backIcon, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
          />
        </TouchableOpacity>
        <Text style={[styles.headerTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
          Search
        </Text>
      </View>
      <TouchableOpacity
        style={[styles.headerAction, { borderColor: dark ? COLORS.dark3 : COLORS.grayscale200 }]}
        onPress={resetFilters}
      >
        <Text style={[styles.headerActionText, { color: dark ? COLORS.white : COLORS.primary }]}>
          Clear
        </Text>
      </TouchableOpacity>
    </View>
  );

  const renderSearchBar = () => (
    <View
      style={[
        styles.searchBarContainer,
        { backgroundColor: dark ? COLORS.dark2 : COLORS.silver },
      ]}
    >
      <TouchableOpacity onPress={() => handleSearch({ searchQuery: searchQuery.trim() })}>
        <Image source={icons.search2} resizeMode="contain" style={styles.searchIcon} />
      </TouchableOpacity>
      <TextInput
        placeholder="Search brand, model, fuel type..."
        placeholderTextColor={COLORS.gray}
        style={[styles.searchInput, { color: dark ? COLORS.white : COLORS.greyscale900 }]}
        value={searchQuery}
        onChangeText={setSearchQuery}
        onSubmitEditing={() => handleSearch({ searchQuery: searchQuery.trim() })}
      />
      {searchQuery.length > 0 ? (
        <TouchableOpacity onPress={() => setSearchQuery('')} style={styles.trailingAction}>
          <FontAwesome name="close" size={16} color={dark ? COLORS.white : COLORS.gray} />
        </TouchableOpacity>
      ) : null}
      <TouchableOpacity onPress={() => refRBSheet.current?.open()} style={styles.filterAction}>
        <Image
          source={icons.filter}
          resizeMode="contain"
          style={[styles.filterIcon, { tintColor: dark ? COLORS.white : COLORS.primary }]}
        />
        {filterCount > 0 ? (
          <View style={styles.filterCounter}>
            <Text style={styles.filterCounterText}>{filterCount}</Text>
          </View>
        ) : null}
      </TouchableOpacity>
    </View>
  );

  const renderActiveFilters = () => {
    if (activeFilters.length === 0) {
      return null;
    }

    return (
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.activeFiltersContainer}
      >
        {activeFilters.map((item) => (
          <TouchableOpacity
            key={item.key}
            onPress={item.onClear}
            style={[
              styles.activeFilterChip,
              { backgroundColor: dark ? COLORS.dark2 : COLORS.tansparentPrimary },
            ]}
          >
            <Text style={[styles.activeFilterText, { color: dark ? COLORS.white : COLORS.primary }]}>
              {item.label}
            </Text>
            <FontAwesome
              name="close"
              size={12}
              color={dark ? COLORS.white : COLORS.primary}
              style={styles.activeFilterClose}
            />
          </TouchableOpacity>
        ))}
      </ScrollView>
    );
  };

  const renderSummary = () => (
    <View style={styles.resultSummary}>
      <View style={styles.resultSummaryLeft}>
        <Text style={[styles.tabText, { color: dark ? COLORS.secondaryWhite : COLORS.black }]}>
          {hasQuery ? `Results for "${searchQuery.trim()}"` : 'Browse Cars'}
        </Text>
        <Text style={[styles.summaryText, { color: dark ? COLORS.greyscale300 : COLORS.grayscale700 }]}>
          {isSearching ? 'Refreshing results...' : browseFeedMode ? `${resultsLabel} ready to explore` : resultsLabel}
        </Text>
      </View>
      <TouchableOpacity
        onPress={() => refRBSheet.current?.open()}
        style={[
          styles.summaryFilterPill,
          { backgroundColor: dark ? COLORS.dark2 : COLORS.tansparentPrimary },
        ]}
      >
        <FontAwesome name="sliders" size={14} color={dark ? COLORS.white : COLORS.primary} />
        <Text style={[styles.summaryFilterText, { color: dark ? COLORS.white : COLORS.primary }]}>
          Filters
        </Text>
      </TouchableOpacity>
    </View>
  );

  const renderResults = () => {
    if (isSearching) {
      return (
        <FlatList
          data={[1, 2, 3, 4]}
          keyExtractor={(item) => item.toString()}
          numColumns={2}
          showsVerticalScrollIndicator={false}
          scrollEnabled={false}
          columnWrapperStyle={styles.gridRow}
          renderItem={() => <SkeletonCard />}
        />
      );
    }

    if (resultsCount > 0) {
      return (
        <FlatList
          data={filteredProducts}
          keyExtractor={(item) => item.id}
          numColumns={2}
          showsVerticalScrollIndicator={false}
          scrollEnabled={false}
          columnWrapperStyle={styles.gridRow}
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
      );
    }

    if (hasQuery || filterCount > 0) {
      return <NotFoundCard />;
    }

    return (
      <Text
        style={[
          styles.emptyStateText,
          { color: dark ? COLORS.secondaryWhite : COLORS.gray },
        ]}
      >
        No cars are available to browse right now.
      </Text>
    );
  };

  const renderCategoryItem = ({ item }: { item: { id: string; name: string } }) => (
    <FilterChip
      label={item.name}
      selected={selectedCategoryId === item.id}
      onPress={() => setSelectedCategoryId(item.id)}
      dark={dark}
    />
  );

  const renderSortItem = ({ item }: { item: { id: string; name: string } }) => (
    <FilterChip
      label={item.name}
      selected={selectedSortId === item.id}
      onPress={() => setSelectedSortId(item.id)}
      dark={dark}
    />
  );

  const renderRatingItem = ({ item }: { item: { id: string; title: string } }) => (
    <FilterChip
      label={item.title === 'All' ? 'All ratings' : `${item.title}+ stars`}
      selected={selectedRatingId === item.id || (!selectedRatingId && item.id === '1')}
      onPress={() => setSelectedRatingId(item.id === '1' ? null : item.id)}
      dark={dark}
      icon={<FontAwesome name="star" size={13} color={selectedRatingId === item.id || (!selectedRatingId && item.id === '1') ? COLORS.white : COLORS.primary} />}
    />
  );

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <ScrollView showsVerticalScrollIndicator={false} contentContainerStyle={styles.contentContainer}>
          {renderSearchBar()}
          {renderActiveFilters()}
          {renderSummary()}
          <View style={styles.resultBody}>{renderResults()}</View>
        </ScrollView>
        <RBSheet
          ref={refRBSheet}
          closeOnPressMask
          height={690}
          customStyles={{
            wrapper: { backgroundColor: 'rgba(0,0,0,0.5)' },
            draggableIcon: { backgroundColor: dark ? COLORS.dark3 : '#000' },
            container: {
              borderTopRightRadius: 32,
              borderTopLeftRadius: 32,
              backgroundColor: dark ? COLORS.dark2 : COLORS.white,
              alignItems: 'center',
            },
          }}
        >
          <Text style={[styles.bottomTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
            Filter Cars
          </Text>
          <View style={styles.separateLine} />
          <ScrollView style={styles.sheetScroll} showsVerticalScrollIndicator={false}>
            <View style={{ width: SIZES.width - 32 }}>
              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Category
              </Text>
              <FlatList
                data={filterCategories}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={renderCategoryItem}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Price Range
              </Text>
              <FlatList
                data={QUICK_PRICE_PRESETS}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={({ item }) => (
                  <FilterChip
                    label={item.label}
                    selected={priceRange[0] === item.values[0] && priceRange[1] === item.values[1]}
                    onPress={() => setPriceRange([...item.values])}
                    dark={dark}
                  />
                )}
                contentContainerStyle={styles.sheetList}
              />
              <MultiSlider
                values={priceRange}
                sliderLength={SIZES.width - 32}
                onValuesChange={(values: number[]) => setPriceRange(values)}
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
              <Text style={[styles.priceRangeText, { color: dark ? COLORS.grayscale100 : COLORS.grayscale700 }]}>
                {`SAR ${priceRange[0].toLocaleString()} - SAR ${priceRange[1].toLocaleString()}`}
              </Text>

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Sort By
              </Text>
              <FlatList
                data={sorts}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={renderSortItem}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Rating
              </Text>
              <FlatList
                data={ratings}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={renderRatingItem}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Fuel Type
              </Text>
              <FlatList
                data={fuelOptions}
                keyExtractor={(item) => item}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={({ item }) => (
                  <FilterChip
                    label={item}
                    selected={selectedFuelType === item}
                    onPress={() => setSelectedFuelType((current) => current === item ? null : item)}
                    dark={dark}
                  />
                )}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Transmission
              </Text>
              <FlatList
                data={transmissionOptions}
                keyExtractor={(item) => item}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={({ item }) => (
                  <FilterChip
                    label={item}
                    selected={selectedTransmission === item}
                    onPress={() => setSelectedTransmission((current) => current === item ? null : item)}
                    dark={dark}
                  />
                )}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Year
              </Text>
              <FlatList
                data={YEAR_PRESETS}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={({ item }) => (
                  <FilterChip
                    label={item.label}
                    selected={selectedYearRange === item.id}
                    onPress={() => setSelectedYearRange((current) => current === item.id ? null : item.id)}
                    dark={dark}
                  />
                )}
                contentContainerStyle={styles.sheetList}
              />

              <Text style={[styles.sheetTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                Mileage
              </Text>
              <FlatList
                data={MILEAGE_PRESETS}
                keyExtractor={(item) => item.id}
                horizontal
                showsHorizontalScrollIndicator={false}
                renderItem={({ item }) => (
                  <FilterChip
                    label={item.label}
                    selected={selectedMileageBand === item.id}
                    onPress={() => setSelectedMileageBand((current) => current === item.id ? null : item.id)}
                    dark={dark}
                  />
                )}
                contentContainerStyle={styles.sheetList}
              />
            </View>
          </ScrollView>

          <View style={styles.separateLine} />

          <View style={styles.bottomContainer}>
            <Button
              title="Reset"
              style={{
                width: (SIZES.width - 32) / 2 - 8,
                backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
                borderRadius: 32,
                borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
              }}
              textColor={dark ? COLORS.white : COLORS.primary}
              onPress={resetFilters}
            />
            <Button
              title="Apply"
              filled
              style={styles.applyButton}
              onPress={applyFilters}
            />
          </View>
        </RBSheet>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: {
    flex: 1,
    backgroundColor: COLORS.white,
  },
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
    padding: 16,
  },
  contentContainer: {
    paddingBottom: 24,
  },
  headerContainer: {
    flexDirection: 'row',
    width: SIZES.width - 32,
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
  },
  headerLeft: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  backIcon: {
    height: 24,
    width: 24,
  },
  headerTitle: {
    fontSize: 20,
    fontFamily: 'bold',
    marginLeft: 16,
  },
  headerAction: {
    minWidth: 62,
    height: 36,
    borderWidth: 1,
    borderRadius: 18,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 12,
  },
  headerActionText: {
    fontSize: 13,
    fontFamily: 'semiBold',
  },
  searchBarContainer: {
    width: SIZES.width - 32,
    backgroundColor: COLORS.secondaryWhite,
    paddingHorizontal: 14,
    borderRadius: 14,
    height: 56,
    marginBottom: 8,
    flexDirection: 'row',
    alignItems: 'center',
  },
  searchIcon: {
    height: 22,
    width: 22,
    tintColor: COLORS.gray,
  },
  searchInput: {
    flex: 1,
    fontSize: 15,
    fontFamily: 'regular',
    marginHorizontal: 10,
  },
  trailingAction: {
    paddingHorizontal: 8,
  },
  filterAction: {
    width: 36,
    height: 36,
    borderRadius: 12,
    alignItems: 'center',
    justifyContent: 'center',
    position: 'relative',
  },
  filterIcon: {
    width: 22,
    height: 22,
  },
  filterCounter: {
    position: 'absolute',
    top: -4,
    right: -2,
    minWidth: 18,
    height: 18,
    borderRadius: 9,
    backgroundColor: COLORS.primary,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 4,
  },
  filterCounterText: {
    fontSize: 10,
    fontFamily: 'bold',
    color: COLORS.white,
  },
  activeFiltersContainer: {
    paddingTop: 4,
    paddingBottom: 6,
  },
  activeFilterChip: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 20,
    paddingHorizontal: 12,
    paddingVertical: 8,
    marginRight: 8,
  },
  activeFilterText: {
    fontSize: 12,
    fontFamily: 'semiBold',
  },
  activeFilterClose: {
    marginLeft: 8,
  },
  resultSummary: {
    flexDirection: 'row',
    alignItems: 'flex-end',
    justifyContent: 'space-between',
    width: SIZES.width - 32,
    marginTop: 8,
    marginBottom: 14,
  },
  resultSummaryLeft: {
    flex: 1,
    paddingRight: 12,
  },
  tabText: {
    fontSize: 20,
    fontFamily: 'semiBold',
    color: COLORS.black,
  },
  summaryText: {
    marginTop: 4,
    fontSize: 13,
    fontFamily: 'medium',
  },
  summaryFilterPill: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 18,
    paddingHorizontal: 12,
    paddingVertical: 8,
  },
  summaryFilterText: {
    fontSize: 12,
    fontFamily: 'semiBold',
    marginLeft: 8,
  },
  resultBody: {
    marginVertical: 4,
  },
  gridRow: {
    gap: 16,
  },
  filterChip: {
    paddingHorizontal: 14,
    paddingVertical: 10,
    marginVertical: 5,
    borderWidth: 1.3,
    borderRadius: 24,
    marginRight: 12,
    flexDirection: 'row',
    alignItems: 'center',
  },
  filterChipSelected: {
    backgroundColor: COLORS.primary,
    borderColor: COLORS.primary,
  },
  filterChipIconWrap: {
    marginRight: 6,
  },
  filterChipText: {
    fontSize: 13,
    fontFamily: 'semiBold',
  },
  bottomContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginVertical: 12,
    paddingHorizontal: 16,
    width: SIZES.width,
  },
  applyButton: {
    width: (SIZES.width - 32) / 2 - 8,
    backgroundColor: COLORS.primary,
    borderRadius: 32,
  },
  bottomTitle: {
    fontSize: 24,
    fontFamily: 'semiBold',
    textAlign: 'center',
    marginTop: 12,
  },
  separateLine: {
    height: 0.4,
    width: SIZES.width - 32,
    backgroundColor: COLORS.greyscale300,
    marginVertical: 12,
  },
  sheetScroll: {
    width: SIZES.width,
    paddingHorizontal: 16,
  },
  sheetList: {
    paddingBottom: 2,
  },
  sheetTitle: {
    fontSize: 18,
    fontFamily: 'semiBold',
    marginVertical: 12,
  },
  priceRangeText: {
    fontSize: 14,
    fontFamily: 'medium',
    marginTop: 4,
  },
  emptyStateText: {
    fontSize: 15,
    fontFamily: 'medium',
    textAlign: 'center',
    lineHeight: 22,
    paddingHorizontal: 16,
    paddingVertical: 24,
  },
});

export default Search;

type SearchFilterState = {
  searchQuery: string;
  selectedCategoryId: string;
  selectedSortId: string;
  selectedRatingId: string | null;
  selectedTransmission: string | null;
  selectedFuelType: string | null;
  selectedYearRange: string | null;
  selectedMileageBand: string | null;
  priceRange: number[];
};

function mapSortIdToApiValue(sortId: string) {
  switch (sortId) {
    case '1':
      return 'popular';
    case '2':
      return 'recent';
    case '3':
      return 'price_desc';
    case '4':
      return 'price_asc';
    case '5':
      return 'rating';
    default:
      return undefined;
  }
}

function normalizeStringParam(value?: string | string[] | null) {
  if (Array.isArray(value)) {
    return value[0] ?? null;
  }

  return value ?? null;
}

function parseNumberParam(value?: string | string[] | null) {
  const normalized = normalizeStringParam(value);
  if (!normalized) {
    return null;
  }

  const parsed = Number(normalized);
  return Number.isFinite(parsed) ? parsed : null;
}

function getUniqueOptions(cars: Car[], key: 'fuelType' | 'transmission') {
  const values = Array.from(new Set(
    cars
      .map((car) => car[key]?.trim())
      .filter((item): item is string => Boolean(item))
  ));

  return values.sort((a, b) => a.localeCompare(b));
}

function applyClientSideFilters(
  cars: Car[],
  options: {
    ratingId: string | null;
    transmission: string | null;
    fuelType: string | null;
    yearRange: string | null;
    mileageBand: string | null;
  }
) {
  let filtered = [...cars];

  if (options.ratingId) {
    const selectedRating = ratings.find((item) => item.id === options.ratingId);
    const minimumRating = selectedRating ? Number(selectedRating.title) : 0;
    if (minimumRating > 0) {
      filtered = filtered.filter((car) => car.rating >= minimumRating);
    }
  }

  if (options.transmission) {
    filtered = filtered.filter((car) => car.transmission === options.transmission);
  }

  if (options.fuelType) {
    filtered = filtered.filter((car) => car.fuelType === options.fuelType);
  }

  if (options.yearRange) {
    const preset = YEAR_PRESETS.find((item) => item.id === options.yearRange);
    if (preset) {
      filtered = filtered.filter((car) => preset.predicate(car));
    }
  }

  if (options.mileageBand) {
    const preset = MILEAGE_PRESETS.find((item) => item.id === options.mileageBand);
    if (preset) {
      filtered = filtered.filter((car) => preset.predicate(car));
    }
  }

  return filtered;
}
