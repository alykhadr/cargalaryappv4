import { View, StyleSheet, Image, TouchableOpacity, FlatList, ListRenderItemInfo, ScrollView, Alert, Linking, Platform } from 'react-native';
import AppAvatar from '@/components/AppAvatar';
import React, { useEffect, useRef, useState } from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView, useSafeAreaInsets } from 'react-native-safe-area-context';
import { COLORS, icons, SIZES } from '@/constants';
import { useTheme } from '@/theme/ThemeProvider';
import SubHeaderItem from '@/components/SubHeaderItem';
import Category from '@/components/Category';
import ProductCard from '@/components/ProductCard';
import SkeletonCard from '@/components/SkeletonCard';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { LinearGradient } from 'expo-linear-gradient';
import { api } from '@/services/api';
import { Car, CompanyInformation, Offer } from '@/types';
import { resolveCarImage, resolveCategoryIcon, resolveRemoteImage } from '@/utils/imageResolver';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'expo-router';
import { useCatalogCategories } from '@/hooks/useCatalogCategories';
import {
  ALL_CATEGORY_ID,
  buildFilterCategories,
  filterCarsByCategory,
  getCategoryPalette,
} from '@/utils/catalog';
import Animated, {
  useSharedValue,
  useAnimatedStyle,
  withTiming,
  withDelay,
  Easing,
} from 'react-native-reanimated';
import { MaterialCommunityIcons } from '@expo/vector-icons';

interface BannerItem {
  id: number;
  discount: string;
  discountName: string;
  bottomTitle: string;
  bottomSubtitle: string;
  imageUrl?: string | null;
  imageKey?: string;
  brandColor: string;
}

interface SearchPreset {
  id: string;
  label: string;
  params: Record<string, string>;
}

const FEATURED_CARD_W = 190;
const FEATURED_CARD_H = 126;

function normalizePhoneForWhatsapp(phone?: string | null) {
  return (phone ?? '').replace(/[^\d]/g, '');
}

function resolveOfferBrandColor(offerId: number) {
  const colors = ['#EB0A1E', '#1A96F0', '#22C55E', '#FF981F', '#2C3E50', '#C084FC'];
  return colors[offerId % colors.length];
}

function formatOfferBanner(offer: Offer): BannerItem {
  const percentage = typeof offer.percentageValue === 'number'
    ? offer.percentageValue.toLocaleString(undefined, { maximumFractionDigits: 2 })
    : null;

  return {
    id: offer.id,
    discount: offer.isPercentage && percentage ? `${percentage}%` : 'LIVE',
    discountName: offer.offerNameEn?.trim() || offer.offerNameAr?.trim() || 'Latest Offer',
    bottomTitle: offer.descriptionEn?.trim() || offer.descriptionAr?.trim() || 'Explore this offer on our latest cars',
    bottomSubtitle: offer.expiredAt
      ? `Valid until ${new Date(offer.expiredAt).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}`
      : 'Limited time availability',
    imageUrl: offer.offerImageUrl,
    brandColor: resolveOfferBrandColor(offer.id),
  };
}

function buildDashboardSearchPresets(cars: Car[], categories: { id: string; name: string }[]): SearchPreset[] {
  const categoryPresets = categories.slice(0, 3).map((item) => ({
    id: `category-${item.id}`,
    label: item.name,
    params: {
      categoryId: item.id,
      title: item.name,
    },
  }));

  const topRatedPreset: SearchPreset = {
    id: 'top-rated',
    label: 'Top Rated',
    params: {
      ratingId: '5',
      sortId: '5',
    },
  };

  const newerModelPreset: SearchPreset = {
    id: 'newer-models',
    label: '2021+',
    params: {
      yearRange: '2021_plus',
      sortId: '2',
    },
  };

  const budgetPreset: SearchPreset = {
    id: 'budget',
    label: 'Under 200k',
    params: {
      maxPrice: '200000',
      sortId: '4',
    },
  };

  const transmissionPresetValue = cars.find((item) => item.transmission?.trim())?.transmission?.trim();
  const transmissionPreset = transmissionPresetValue
    ? [{
        id: 'transmission',
        label: transmissionPresetValue,
        params: {
          transmission: transmissionPresetValue,
        },
      }]
    : [];

  return [...categoryPresets, budgetPreset, newerModelPreset, topRatedPreset, ...transmissionPreset];
}

const Home = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const router = useRouter();
  const insets = useSafeAreaInsets();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedCategoryId, setSelectedCategoryId] = useState(ALL_CATEGORY_ID);
  const { dark, colors } = useTheme();
  const { user, isGuest } = useAuth();
  const [cars, setCars] = useState<Car[]>([]);
  const [carsLoading, setCarsLoading] = useState(true);
  const [companyInfo, setCompanyInfo] = useState<CompanyInformation | null>(null);
  const [offers, setOffers] = useState<Offer[]>([]);
  const [offersLoaded, setOffersLoaded] = useState(false);
  const { categories, loading: categoriesLoading } = useCatalogCategories(cars);
  const bannerRef = useRef<FlatList>(null);
  const autoScrollIdx = useRef(0);
  const [bannerReady, setBannerReady] = useState(false);
  const [bannerContentReady, setBannerContentReady] = useState(false);
  const bannerWidth = SIZES.width - 32;
  const searchPresets = buildDashboardSearchPresets(cars, categories);

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
    api.getCompanyInformation()
      .then(setCompanyInfo)
      .catch(() => {});
  }, []);

  useEffect(() => {
    api.getOffers()
      .then(setOffers)
      .catch(() => setOffers([]))
      .finally(() => setOffersLoaded(true));
  }, []);

  const bannerItems: BannerItem[] = offers.map(formatOfferBanner);

  useEffect(() => {
    if (!bannerReady || !bannerContentReady || bannerItems.length <= 1) {
      return;
    }

    const timer = setInterval(() => {
      const next = (autoScrollIdx.current + 1) % bannerItems.length;
      autoScrollIdx.current = next;
      setCurrentIndex(next);
      requestAnimationFrame(() => {
        bannerRef.current?.scrollToOffset({
          offset: bannerWidth * next,
          animated: true,
        });
      });
    }, 3500);
    return () => clearInterval(timer);
  }, [bannerItems.length, bannerReady, bannerContentReady, bannerWidth]);

  useEffect(() => {
    autoScrollIdx.current = 0;
    setCurrentIndex(0);
    requestAnimationFrame(() => {
      bannerRef.current?.scrollToOffset({
        offset: 0,
        animated: false,
      });
    });
  }, [bannerItems.length]);

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

  const handleWhatsappPress = async () => {
    const phone = normalizePhoneForWhatsapp(companyInfo?.mobileNo || companyInfo?.telNo);
    if (!phone) {
      Alert.alert('WhatsApp unavailable', 'No WhatsApp contact number is configured yet.');
      return;
    }

    const companyName = companyInfo?.companyNameEn?.trim() || 'Car Gallery';
    const message = encodeURIComponent(`Hello ${companyName}, I want to ask about a car.`);
    const url = `https://wa.me/${phone}?text=${message}`;

    try {
      const supported = await Linking.canOpenURL(url);
      if (!supported) {
        Alert.alert('WhatsApp unavailable', 'Unable to open WhatsApp on this device.');
        return;
      }

      await Linking.openURL(url);
    } catch {
      Alert.alert('WhatsApp unavailable', 'Unable to start WhatsApp chat right now.');
    }
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
    <View>
      <TouchableOpacity
        onPress={() => router.push('/search')}
        style={[styles.searchBarContainer, {
          backgroundColor: dark ? COLORS.dark2 : '#F7F7F7',
          borderColor: dark ? COLORS.dark3 : 'rgba(0,0,0,0.06)',
          shadowOpacity: dark ? 0.2 : 0.07,
        }]}
      >
        <Image source={icons.search} resizeMode="contain" style={styles.searchIcon} />
        <Text numberOfLines={1} style={styles.searchInput}>Search brand, model, fuel, year...</Text>
        <View style={styles.filterBadge}>
          <Image source={icons.filter} resizeMode="contain" style={styles.filterIcon} />
        </View>
      </TouchableOpacity>
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.searchPresetRow}
      >
        {searchPresets.map((item) => (
          <TouchableOpacity
            key={item.id}
            activeOpacity={0.86}
            style={[
              styles.searchPresetChip,
              {
                backgroundColor: dark ? COLORS.dark2 : '#FFFFFF',
                borderColor: dark ? COLORS.dark3 : 'rgba(232,0,28,0.14)',
              },
            ]}
            onPress={() => router.push({ pathname: '/search', params: item.params })}
          >
            <Text style={[styles.searchPresetText, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
              {item.label}
            </Text>
          </TouchableOpacity>
        ))}
      </ScrollView>
    </View>
  );

  /* ── Banner ── */
  const renderBannerItem = ({ item }: ListRenderItemInfo<BannerItem>) => (
    <View style={[styles.bannerSlide, { width: bannerWidth }]}>
      <View style={styles.bannerShadow}>
        <View style={styles.bannerContainer}>
          <Image
            source={item.imageUrl ? resolveRemoteImage(item.imageUrl) : resolveCarImage(item.imageKey || '')}
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
            <Text style={styles.bannerMeta}>{item.bottomSubtitle}</Text>
          </View>
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
          backgroundColor: bannerItems[index]?.brandColor ?? COLORS.primary,
        },
      ]}
    />
  );

  const renderBannerLoading = () => (
    <View style={styles.bannerItemContainer}>
      <View
        style={[
          styles.bannerShadow,
          styles.bannerLoadingShell,
          { backgroundColor: dark ? COLORS.dark2 : '#EFF2F6' },
        ]}
      >
        <View style={[styles.bannerLoadingBadge, { backgroundColor: dark ? COLORS.dark3 : '#DEE5EE' }]} />
        <View style={[styles.bannerLoadingTitle, { backgroundColor: dark ? COLORS.dark3 : '#DEE5EE' }]} />
        <View style={[styles.bannerLoadingSubtitle, { backgroundColor: dark ? COLORS.dark3 : '#DEE5EE' }]} />
        <View style={[styles.bannerLoadingMeta, { backgroundColor: dark ? COLORS.dark3 : '#DEE5EE' }]} />
      </View>
    </View>
  );

  const renderBanner = () => {
    if (!offersLoaded) {
      return renderBannerLoading();
    }

    if (bannerItems.length === 0) {
      return null;
    }

    return (
      <View style={styles.bannerItemContainer}>
        <FlatList
          ref={bannerRef}
          data={bannerItems}
          renderItem={renderBannerItem}
          keyExtractor={keyExtractor}
          horizontal
          pagingEnabled
          showsHorizontalScrollIndicator={false}
          removeClippedSubviews={false}
          scrollEventThrottle={16}
          onMomentumScrollEnd={(event) => {
            const newIndex = Math.round(
              event.nativeEvent.contentOffset.x / bannerWidth
            );
            autoScrollIdx.current = newIndex;
            setCurrentIndex(newIndex);
          }}
          getItemLayout={(_, index) => ({
            length: bannerWidth,
            offset: bannerWidth * index,
            index,
          })}
          onLayout={() => setBannerReady(true)}
          onContentSizeChange={() => setBannerContentReady(true)}
        />
        <View style={styles.dotContainer}>
          {bannerItems.map((_, i) => renderDot(i))}
        </View>
      </View>
    );
  };

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
        onPress={() => router.push('/categories')}
      />
      <View style={styles.categoryGrid}>
        {categoriesLoading ? Array.from({ length: 8 }).map((_, index) => (
          <View key={`category-skeleton-${index}`} style={styles.categoryPlaceholder}>
            <View style={[styles.categoryPlaceholderIcon, { backgroundColor: dark ? COLORS.dark3 : '#EEF1F6' }]} />
            <View style={[styles.categoryPlaceholderLabel, { backgroundColor: dark ? COLORS.dark3 : '#EEF1F6' }]} />
          </View>
        )) : categories.slice(0, 8).map((item) => {
          const palette = getCategoryPalette(item.id);
          return (
            <Category
              key={item.id}
              name={item.name}
              icon={resolveCategoryIcon(item.imageUrl, item.brandLogoKey)}
              preserveIconColor={Boolean(item.imageUrl || item.brandLogoKey)}
              iconColor={palette.iconColor}
              backgroundColor={palette.backgroundColor}
              onPress={() => router.push({ pathname: '/category/[id]', params: { id: item.id, title: item.name } })}
            />
          );
        })}
      </View>
    </View>
  );

  /* ── Top Deals ── */
  const renderPopularProducts = () => {
    const filteredCars = filterCarsByCategory(cars, selectedCategoryId);
    const filterCategories = buildFilterCategories(categories);

    return (
      <View>
        <SubHeaderItem
          title="Top Deals"
          navTitle="See All"
          onPress={() => navigation.navigate('mostpopularproducts')}
        />
        <FlatList
          data={filterCategories}
          keyExtractor={item => item.id}
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={{ paddingBottom: 4 }}
          renderItem={({ item }) => {
            const active = selectedCategoryId === item.id;
            return (
              <TouchableOpacity
                onPress={() => setSelectedCategoryId(item.id)}
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
            <View style={styles.productGrid}>
              {[1, 2, 3, 4, 5, 6].map(item => <SkeletonCard key={item} />)}
            </View>
          ) : filteredCars.length === 0 ? (
            <Text style={[styles.emptyStateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
              No cars available in this category yet.
            </Text>
          ) : (
            <View style={styles.productGrid}>
              {filteredCars.map(item => (
                <ProductCard
                  key={item.id}
                  carId={item.id}
                  name={item.name}
                  image={resolveCarImage(item.imageKey)}
                  numSolds={item.numSolds}
                  rating={item.rating}
                  price={item.price}
                  onPress={() => navigation.navigate('cardetails', { carId: item.id })}
                />
              ))}
            </View>
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
          <ScrollView
            showsVerticalScrollIndicator={false}
            keyboardShouldPersistTaps="handled"
            contentContainerStyle={styles.scrollContent}
          >
            {renderSearchBar()}
            {renderBanner()}
            {renderFeatured()}
            {renderCategories()}
            {renderPopularProducts()}
          </ScrollView>
        </Animated.View>
        <TouchableOpacity
          activeOpacity={0.9}
          style={[
            styles.whatsappFab,
            { bottom: (Platform.OS === 'ios' ? 88 : 64) + insets.bottom + 14 },
          ]}
          onPress={handleWhatsappPress}
        >
          <MaterialCommunityIcons name="whatsapp" size={28} color={COLORS.white} />
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: { flex: 1, backgroundColor: COLORS.white },
  container: { flex: 1, backgroundColor: COLORS.white, padding: 16 },
  scrollContent: { paddingBottom: 96 },
  whatsappFab: {
    position: 'absolute',
    right: 18,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: '#25D366',
    alignItems: 'center',
    justifyContent: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 10 },
    shadowOpacity: 0.18,
    shadowRadius: 16,
    elevation: 10,
    borderWidth: 3,
    borderColor: 'rgba(255,255,255,0.92)',
  },

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
  searchPresetRow: {
    paddingBottom: 4,
    marginBottom: 8,
  },
  searchPresetChip: {
    paddingHorizontal: 14,
    paddingVertical: 9,
    borderRadius: 18,
    borderWidth: 1,
    marginRight: 10,
  },
  searchPresetText: {
    fontSize: 12,
    fontFamily: 'semiBold',
  },
  searchIcon: { height: 20, width: 20, tintColor: COLORS.gray },
  searchInput: { flex: 1, fontSize: 15, fontFamily: 'regular', marginHorizontal: 10, color: COLORS.gray },
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
  bannerSlide: {
    width: SIZES.width - 32,
  },
  bannerLoadingShell: {
    width: SIZES.width - 32,
    height: 170,
    borderRadius: 30,
    paddingHorizontal: 22,
    paddingTop: 28,
    justifyContent: 'flex-start',
  },
  bannerLoadingBadge: {
    width: 82,
    height: 30,
    borderRadius: 15,
    marginBottom: 18,
  },
  bannerLoadingTitle: {
    width: '58%',
    height: 28,
    borderRadius: 14,
    marginBottom: 14,
  },
  bannerLoadingSubtitle: {
    width: '72%',
    height: 18,
    borderRadius: 9,
    marginBottom: 12,
  },
  bannerLoadingMeta: {
    width: '44%',
    height: 16,
    borderRadius: 8,
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
  bannerMeta: {
    fontSize: 11,
    fontFamily: 'regular',
    color: 'rgba(255,255,255,0.70)',
    marginTop: 6,
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

  categoryGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  categoryPlaceholder: {
    width: (SIZES.width - 32) / 4,
    alignItems: 'center',
    marginBottom: 12,
  },
  categoryPlaceholderIcon: {
    width: 54,
    height: 54,
    borderRadius: 999,
    marginBottom: 8,
  },
  categoryPlaceholderLabel: {
    width: 56,
    height: 12,
    borderRadius: 999,
  },

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
  productGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    columnGap: 16,
  },
  emptyStateText: {
    fontSize: 14,
    fontFamily: 'medium',
    paddingVertical: 18,
  },
});

export default Home;
