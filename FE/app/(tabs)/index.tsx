import { View, StyleSheet, Image, TouchableOpacity, FlatList, ListRenderItemInfo } from 'react-native';
import AppAvatar from '@/components/AppAvatar';
import React, { useEffect, useRef, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { COLORS, icons, images, SIZES } from '@/constants';
import { useTheme } from '@/theme/ThemeProvider';
import { banners, categories } from '@/data';
import SubHeaderItem from '@/components/SubHeaderItem';
import Category from '@/components/Category';
import ProductCard from '@/components/ProductCard';
import SkeletonCard from '@/components/SkeletonCard';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { LinearGradient } from 'expo-linear-gradient';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage } from '@/utils/imageResolver';
import { useAuth } from '@/context/AuthContext';
import Animated, {
  useSharedValue,
  useAnimatedStyle,
  withTiming,
  withDelay,
  Easing,
} from 'react-native-reanimated';

interface BannerItem {
  id: number;
  discount: string;
  discountName: string;
  bottomTitle: string;
  bottomSubtitle: string;
  imageKey: string;
  brandColor: string;
}

const FEATURED_CARD_W = 190;
const FEATURED_CARD_H = 126;

const Home = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedCategories, setSelectedCategories] = useState(["0"]);
  const { dark, colors } = useTheme();
  const { user, isGuest } = useAuth();
  const [cars, setCars] = useState<Car[]>([]);
  const [carsLoading, setCarsLoading] = useState(true);
  const bannerRef = useRef<FlatList>(null);
  const autoScrollIdx = useRef(0);

  const headerOpacity = useSharedValue(0);
  const headerY = useSharedValue(-16);
  const contentOpacity = useSharedValue(0);
  const contentY = useSharedValue(24);

  useEffect(() => {
    const cfg = { duration: 520, easing: Easing.out(Easing.cubic) };
    headerOpacity.value = withTiming(1, { ...cfg, duration: 400 });
    headerY.value = withTiming(0, { ...cfg, duration: 400 });
    contentOpacity.value = withDelay(180, withTiming(1, cfg));
    contentY.value = withDelay(180, withTiming(0, cfg));
  }, []);

  const headerStyle = useAnimatedStyle(() => ({
    opacity: headerOpacity.value,
    transform: [{ translateY: headerY.value }],
  }));

  const contentStyle = useAnimatedStyle(() => ({
    opacity: contentOpacity.value,
    transform: [{ translateY: contentY.value }],
  }));

  useEffect(() => {
    api.getCars({ limit: '20' })
      .then(res => setCars(res.cars))
      .catch(() => {})
      .finally(() => setCarsLoading(false));
  }, []);

  useEffect(() => {
    const timer = setInterval(() => {
      const next = (autoScrollIdx.current + 1) % (banners as any[]).length;
      autoScrollIdx.current = next;
      setCurrentIndex(next);
      bannerRef.current?.scrollToIndex({ index: next, animated: true });
    }, 3500);
    return () => clearInterval(timer);
  }, []);

  const handleWishlistPress = () => {
    if (isGuest || !user) navigation.navigate('login');
    else navigation.navigate('mywishlist');
  };

  const getGreeting = () => {
    const h = new Date().getHours();
    if (h < 12) return 'Good Morning';
    if (h < 17) return 'Good Afternoon';
    return 'Good Evening';
  };

/* ── Header ── */
  const renderHeader = () => (
    <Animated.View style={[styles.headerContainer, headerStyle]}>
      <View style={styles.viewLeft}>
        <View style={styles.avatarWrap}>
          <AppAvatar uri={user?.avatarUrl ?? null} size={50} />
          <View style={[styles.onlineDot, { borderColor: colors.background }]} />
        </View>
        <View style={styles.viewNameContainer}>
          <View style={styles.locationRow}>
            <Image
              source={icons.location}
              style={[styles.locationIcon, { tintColor: COLORS.primary }]}
            />
            <Text style={styles.locationText}>Riyadh, KSA</Text>
          </View>
          <Text style={[styles.greetingText, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
            {getGreeting()} 👋
          </Text>
          <Text style={styles.nameText}>{user?.fullName || 'Guest'}</Text>
        </View>
      </View>
      <View style={styles.viewRight}>
        <TouchableOpacity
          style={[styles.iconBtn, { backgroundColor: dark ? COLORS.dark3 : '#F2F2F2' }]}
          onPress={() => navigation.navigate('notifications')}
        >
          <Image
            source={icons.bellOutline}
            style={[styles.headerIcon, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
          />
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.iconBtn, { backgroundColor: dark ? COLORS.dark3 : '#F2F2F2', marginLeft: 8 }]}
          onPress={handleWishlistPress}
        >
          <Image
            source={icons.heartOutline}
            style={[styles.headerIcon, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
          />
        </TouchableOpacity>
      </View>
    </Animated.View>
  );

  /* ── Search bar ── */
  const renderSearchBar = () => (
    <TouchableOpacity
      onPress={() => navigation.navigate('search')}
      style={[styles.searchBarContainer, {
        backgroundColor: dark ? COLORS.dark2 : '#F7F7F7',
        borderColor: dark ? COLORS.dark3 : 'rgba(0,0,0,0.06)',
        shadowOpacity: dark ? 0.2 : 0.07,
      }]}
    >
      <Image source={icons.search} resizeMode="contain" style={styles.searchIcon} />
      <TextInput
        placeholder="Search by brand, model..."
        placeholderTextColor={COLORS.gray}
        style={styles.searchInput}
        onFocus={() => navigation.navigate('search')}
      />
      <View style={styles.filterBadge}>
        <Image source={icons.filter} resizeMode="contain" style={styles.filterIcon} />
      </View>
    </TouchableOpacity>
  );

  /* ── Banner ── */
  const renderBannerItem = ({ item }: ListRenderItemInfo<BannerItem>) => (
    <View style={styles.bannerShadow}>
      <View style={styles.bannerContainer}>
        <Image
          source={resolveCarImage(item.imageKey)}
          resizeMode="cover"
          style={StyleSheet.absoluteFillObject}
        />
        <View style={[StyleSheet.absoluteFillObject, { backgroundColor: 'rgba(0,0,0,0.28)' }]} />
        <LinearGradient
          colors={['transparent', 'rgba(0,0,0,0.72)', 'rgba(0,0,0,0.92)']}
          locations={[0.18, 0.62, 1]}
          style={StyleSheet.absoluteFillObject}
        />
        <View style={[styles.bannerTopLine, { backgroundColor: item.brandColor }]} />
        <View style={styles.bannerContent}>
          <View style={[styles.bannerBadge, { backgroundColor: item.brandColor }]}>
            <Text style={styles.bannerBadgeText}>{item.discount} OFF</Text>
          </View>
          <Text style={styles.bannerTitle}>{item.discountName}</Text>
          <Text style={styles.bannerSubtitle}>{item.bottomTitle}</Text>
        </View>
      </View>
    </View>
  );

  const keyExtractor = (item: { id: number | string }) => item.id.toString();

  const renderDot = (index: number) => (
    <View
      key={index}
      style={[
        styles.dot,
        index === currentIndex && {
          width: 20,
          borderRadius: 5,
          backgroundColor: (banners as any[])[index]?.brandColor ?? COLORS.primary,
        },
      ]}
    />
  );

  const renderBanner = () => (
    <View style={styles.bannerItemContainer}>
      <FlatList
        ref={bannerRef}
        data={banners as BannerItem[]}
        renderItem={renderBannerItem}
        keyExtractor={keyExtractor}
        horizontal
        pagingEnabled
        showsHorizontalScrollIndicator={false}
        scrollEventThrottle={16}
        onMomentumScrollEnd={(event) => {
          const newIndex = Math.round(
            event.nativeEvent.contentOffset.x / (SIZES.width - 32)
          );
          autoScrollIdx.current = newIndex;
          setCurrentIndex(newIndex);
        }}
      />
      <View style={styles.dotContainer}>
        {(banners as any[]).map((_, i) => renderDot(i))}
      </View>
    </View>
  );

  /* ── Featured horizontal scroll ── */
  const renderFeatured = () => {
    if (carsLoading || cars.length === 0) return null;
    return (
      <View>
        <SubHeaderItem
          title="Featured"
          navTitle="See All"
          onPress={() => navigation.navigate('mostpopularproducts')}
        />
        <FlatList
          data={cars.slice(0, 6)}
          keyExtractor={item => `feat-${item.id}`}
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={{ paddingRight: 4 }}
          renderItem={({ item }) => (
            <TouchableOpacity
              activeOpacity={0.88}
              onPress={() => navigation.navigate('cardetails', { carId: item.id })}
              style={styles.featuredCard}
            >
              <Image
                source={resolveCarImage(item.imageKey)}
                resizeMode="cover"
                style={StyleSheet.absoluteFillObject}
              />
              <LinearGradient
                colors={['transparent', 'rgba(0,0,0,0.82)']}
                locations={[0.38, 1]}
                style={StyleSheet.absoluteFillObject}
              />
              <View style={styles.featuredContent}>
                <Text style={styles.featuredName} numberOfLines={1}>{item.name}</Text>
                <Text style={styles.featuredPrice}>SAR {item.price}</Text>
              </View>
            </TouchableOpacity>
          )}
        />
      </View>
    );
  };

  /* ── Categories ── */
  const renderCategories = () => (
    <View>
      <SubHeaderItem
        title="Categories"
        navTitle="See all"
        onPress={() => navigation.navigate('categories')}
      />
      <FlatList
        data={categories.slice(1, 9)}
        keyExtractor={(_, index) => index.toString()}
        numColumns={4}
        renderItem={({ item }) => (
          <Category
            name={item.name}
            icon={item.icon}
            iconColor={item.iconColor}
            backgroundColor={item.backgroundColor}
            onPress={item.onPress ? () => navigation.navigate(item.onPress) : undefined}
          />
        )}
      />
    </View>
  );

  /* ── Top Deals ── */
  const renderPopularProducts = () => {
    const filteredCars = cars.filter(car =>
      selectedCategories.includes('0') || selectedCategories.includes(car.categoryId)
    );

    const toggleCategory = (id: string) => {
      const updated = [...selectedCategories];
      const idx = updated.indexOf(id);
      if (idx === -1) updated.push(id);
      else updated.splice(idx, 1);
      setSelectedCategories(updated);
    };

    return (
      <View>
        <SubHeaderItem
          title="Top Deals"
          navTitle="See All"
          onPress={() => navigation.navigate('mostpopularproducts')}
        />
        <FlatList
          data={categories}
          keyExtractor={item => item.id}
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={{ paddingBottom: 4 }}
          renderItem={({ item }) => {
            const active = selectedCategories.includes(item.id);
            return (
              <TouchableOpacity
                onPress={() => toggleCategory(item.id)}
                style={[
                  styles.chip,
                  active
                    ? { backgroundColor: COLORS.primary, borderColor: COLORS.primary }
                    : { backgroundColor: 'transparent', borderColor: dark ? COLORS.dark3 : COLORS.primary },
                ]}
              >
                <Text style={[
                  styles.chipText,
                  { color: active ? COLORS.white : dark ? COLORS.white : COLORS.primary },
                ]}>
                  {item.name}
                </Text>
              </TouchableOpacity>
            );
          }}
        />
        <View style={{ marginTop: 12 }}>
          {carsLoading ? (
            <FlatList
              data={[1, 2, 3, 4, 5, 6]}
              keyExtractor={item => item.toString()}
              numColumns={2}
              columnWrapperStyle={styles.cardRow}
              scrollEnabled={false}
              renderItem={() => <SkeletonCard />}
            />
          ) : (
            <FlatList
              data={filteredCars}
              keyExtractor={item => item.id}
              numColumns={2}
              columnWrapperStyle={styles.cardRow}
              scrollEnabled={false}
              renderItem={({ item }) => (
                <ProductCard
                  name={item.name}
                  image={resolveCarImage(item.imageKey)}
                  numSolds={item.numSolds}
                  rating={item.rating}
                  price={item.price}
                  onPress={() => navigation.navigate('cardetails', { carId: item.id })}
                />
              )}
            />
          )}
        </View>
      </View>
    );
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <Animated.View style={[{ flex: 1 }, contentStyle]}>
          <ScrollView showsVerticalScrollIndicator={false}>
            {renderSearchBar()}
            {renderBanner()}
            {renderFeatured()}
            {renderCategories()}
            {renderPopularProducts()}
          </ScrollView>
        </Animated.View>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: { flex: 1, backgroundColor: COLORS.white },
  container: { flex: 1, backgroundColor: COLORS.white, padding: 16 },

  /* Header */
  headerContainer: {
    flexDirection: 'row',
    width: SIZES.width - 32,
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 4,
  },
  viewLeft: { flexDirection: 'row', alignItems: 'center' },
  avatarWrap: { position: 'relative' },
  onlineDot: {
    position: 'absolute',
    bottom: -2,
    right: -2,
    width: 13,
    height: 13,
    borderRadius: 7,
    backgroundColor: '#22C55E',
    borderWidth: 2,
  },
  viewNameContainer: { marginLeft: 12 },
  locationRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 2 },
  locationIcon: { width: 11, height: 11, marginRight: 3 },
  locationText: { fontSize: 11, fontFamily: 'medium', color: COLORS.primary },
  greetingText: { fontSize: 18, fontFamily: 'bold', color: COLORS.greyscale900 },
  nameText: { fontSize: 12, fontFamily: 'regular', color: 'gray', marginTop: 1 },
  viewRight: { flexDirection: 'row', alignItems: 'center' },
  iconBtn: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
  headerIcon: { width: 20, height: 20 },

  /* Search */
  searchBarContainer: {
    width: SIZES.width - 32,
    paddingHorizontal: 14,
    borderRadius: 14,
    height: 52,
    marginVertical: 14,
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowRadius: 8,
    elevation: 2,
  },
  searchIcon: { height: 20, width: 20, tintColor: COLORS.gray },
  searchInput: { flex: 1, fontSize: 15, fontFamily: 'regular', marginHorizontal: 10 },
  filterBadge: {
    width: 34,
    height: 34,
    borderRadius: 10,
    backgroundColor: COLORS.primary,
    alignItems: 'center',
    justifyContent: 'center',
  },
  filterIcon: { width: 18, height: 18, tintColor: '#FFFFFF' },

  /* Banner */
  bannerShadow: {
    width: SIZES.width - 32,
    borderRadius: 24,
    backgroundColor: '#111',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.22,
    shadowRadius: 18,
    elevation: 10,
  },
  bannerContainer: {
    width: SIZES.width - 32,
    height: 170,
    borderRadius: 24,
    overflow: 'hidden',
    backgroundColor: '#111',
  },
  bannerTopLine: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    height: 4,
    borderTopLeftRadius: 24,
    borderTopRightRadius: 24,
  },
  bannerContent: { position: 'absolute', left: 18, right: 18, bottom: 16 },
  bannerBadge: {
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 20,
    alignSelf: 'flex-start',
    marginBottom: 8,
  },
  bannerBadgeText: { fontSize: 11, fontFamily: 'bold', color: COLORS.white },
  bannerTitle: {
    fontSize: 19,
    fontFamily: 'bold',
    color: COLORS.white,
    marginBottom: 3,
    textShadowColor: 'rgba(0,0,0,0.5)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 4,
  },
  bannerSubtitle: {
    fontSize: 12,
    fontFamily: 'medium',
    color: 'rgba(255,255,255,0.82)',
  },
  bannerItemContainer: { width: '100%', paddingBottom: 4, height: 192 },
  dotContainer: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 10,
  },
  dot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    backgroundColor: '#ccc',
    marginHorizontal: 4,
  },

  /* Featured */
  featuredCard: {
    width: FEATURED_CARD_W,
    height: FEATURED_CARD_H,
    borderRadius: 16,
    overflow: 'hidden',
    marginRight: 12,
    backgroundColor: '#1a1a1a',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.18,
    shadowRadius: 10,
    elevation: 6,
  },
  featuredContent: { position: 'absolute', bottom: 10, left: 10, right: 10 },
  featuredName: {
    fontSize: 13,
    fontFamily: 'semiBold',
    color: '#FFFFFF',
    marginBottom: 2,
    textShadowColor: 'rgba(0,0,0,0.6)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 3,
  },
  featuredPrice: { fontSize: 12, fontFamily: 'bold', color: COLORS.primary },

  /* Chips */
  chip: {
    paddingHorizontal: 14,
    paddingVertical: 8,
    marginRight: 10,
    borderWidth: 1.3,
    borderRadius: 24,
    marginBottom: 2,
  },
  chipText: { fontSize: 13, fontFamily: 'semiBold' },

  /* Card grid */
  cardRow: { gap: 16 },
});

export default Home;
