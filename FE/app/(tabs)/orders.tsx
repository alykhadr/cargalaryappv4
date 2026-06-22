import { View, StyleSheet, FlatList, Image, TouchableOpacity } from 'react-native';
import React, { useEffect, useState } from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTheme } from '@/theme/ThemeProvider';
import { COLORS, icons, images, SIZES } from '@/constants';
import { useAuth } from '@/context/AuthContext';
import { api } from '@/services/api';
import { resolveCarImage } from '@/utils/imageResolver';
import { Car, Inquiry } from '@/types';
import { useNavigation } from 'expo-router';
import { NavigationProp } from '@react-navigation/native';
import { FontAwesome } from '@expo/vector-icons';
import { LinearGradient } from 'expo-linear-gradient';

type InquiryWithCar = Inquiry & { car: Car | null };

const formatDate = (iso: string) => {
  const d = new Date(iso);
  return d.toLocaleDateString('en-US', { day: 'numeric', month: 'short', year: 'numeric' });
};

const MyInquiries = () => {
  const { dark, colors } = useTheme();
  const { user, isGuest } = useAuth();
  const navigation = useNavigation<NavigationProp<any>>();
  const [inquiries, setInquiries] = useState<InquiryWithCar[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (isGuest || !user) { setLoading(false); return; }
    api.getInquiries()
      .then(res => setInquiries(res.inquiries))
      .catch(() => {})
      .finally(() => setLoading(false));
  }, [user, isGuest]);

  const renderHeader = () => (
    <View style={styles.headerContainer}>
      <Image
        source={images.logo}
        resizeMode="contain"
        style={[styles.logo, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
      />
      <Text style={[styles.headerTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
        My Inquiries
      </Text>
    </View>
  );

  /* ── Guest guard ── */
  if (!loading && (isGuest || !user)) {
    return (
      <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
        <View style={[styles.container, { backgroundColor: colors.background }]}>
          {renderHeader()}
          <View style={styles.centerState}>
            <LinearGradient
              colors={['#FF6B35', '#E8001C']}
              start={{ x: 0, y: 0 }} end={{ x: 1, y: 1 }}
              style={styles.guestIconBox}
            >
              <FontAwesome name="file-text-o" size={30} color="#FFF" />
            </LinearGradient>
            <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
              Login to see your inquiries
            </Text>
            <Text style={styles.emptySubtitle}>
              {"Sign in to track all the car inquiries you've submitted to dealers."}
            </Text>
            <TouchableOpacity
              style={styles.loginBtn}
              onPress={() => navigation.navigate('login' as never)}
            >
              <Text style={styles.loginBtnText}>Login</Text>
            </TouchableOpacity>
          </View>
        </View>
      </SafeAreaView>
    );
  }

  /* ── Empty state ── */
  if (!loading && inquiries.length === 0) {
    return (
      <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
        <View style={[styles.container, { backgroundColor: colors.background }]}>
          {renderHeader()}
          <View style={styles.centerState}>
            <LinearGradient
              colors={['#FF6B35', '#E8001C']}
              start={{ x: 0, y: 0 }} end={{ x: 1, y: 1 }}
              style={styles.guestIconBox}
            >
              <FontAwesome name="file-text-o" size={30} color="#FFF" />
            </LinearGradient>
            <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
              No inquiries yet
            </Text>
            <Text style={styles.emptySubtitle}>
              {'Browse cars and tap "Contact Dealer" to send your first inquiry.'}
            </Text>
            <TouchableOpacity
              style={styles.loginBtn}
              onPress={() => navigation.navigate('index' as never)}
            >
              <Text style={styles.loginBtnText}>Browse Cars</Text>
            </TouchableOpacity>
          </View>
        </View>
      </SafeAreaView>
    );
  }

  const renderItem = ({ item }: { item: InquiryWithCar }) => (
    <TouchableOpacity
      activeOpacity={0.85}
      onPress={() => item.car && navigation.navigate('cardetails', { carId: item.car.id })}
      style={[styles.card, { backgroundColor: dark ? COLORS.dark2 : '#FAFAFA',
        borderColor: dark ? COLORS.dark3 : 'rgba(0,0,0,0.07)' }]}
    >
      {/* Car thumbnail */}
      <View style={styles.thumbWrap}>
        {item.car ? (
          <Image
            source={resolveCarImage(item.car.imageKey)}
            resizeMode="cover"
            style={styles.thumb}
          />
        ) : (
          <View style={[styles.thumb, styles.thumbPlaceholder]}>
            <FontAwesome name="car" size={22} color={COLORS.gray} />
          </View>
        )}
      </View>

      {/* Details */}
      <View style={styles.cardBody}>
        <Text style={[styles.carName, { color: dark ? COLORS.white : COLORS.greyscale900 }]} numberOfLines={1}>
          {item.car?.name ?? 'Car Unavailable'}
        </Text>
        {item.car && (
          <Text style={styles.carPrice}>SAR {item.car.price}</Text>
        )}
        <Text style={[styles.message, { color: dark ? COLORS.grayscale400 : '#666' }]} numberOfLines={2}>
          {`"${item.message}"`}
        </Text>
        <View style={styles.cardFooter}>
          <Text style={[styles.dateText, { color: dark ? COLORS.grayscale400 : '#999' }]}>
            {formatDate(item.createdAt)}
          </Text>
          <View style={styles.pendingBadge}>
            <Text style={styles.pendingText}>Pending</Text>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <FlatList
          data={inquiries}
          keyExtractor={item => item.id}
          renderItem={renderItem}
          contentContainerStyle={{ paddingBottom: 80 }}
          showsVerticalScrollIndicator={false}
        />
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: { flex: 1 },
  container: { flex: 1, paddingHorizontal: 16, paddingTop: 8 },
  headerContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 20,
    marginTop: 4,
  },
  logo: { width: 28, height: 28, tintColor: COLORS.primary },
  headerTitle: {
    fontSize: 22,
    fontFamily: 'bold',
    marginLeft: 10,
  },

  /* Card */
  card: {
    flexDirection: 'row',
    borderRadius: 16,
    borderWidth: 1,
    padding: 12,
    marginBottom: 14,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 8,
    elevation: 3,
  },
  thumbWrap: {
    borderRadius: 12,
    overflow: 'hidden',
    marginRight: 12,
  },
  thumb: { width: 84, height: 84, borderRadius: 12 },
  thumbPlaceholder: {
    backgroundColor: '#EFEFEF',
    alignItems: 'center',
    justifyContent: 'center',
  },
  cardBody: { flex: 1, justifyContent: 'space-between' },
  carName: { fontSize: 15, fontFamily: 'bold', marginBottom: 2 },
  carPrice: { fontSize: 13, fontFamily: 'semiBold', color: COLORS.primary, marginBottom: 4 },
  message: { fontSize: 12, fontFamily: 'regular', lineHeight: 17, marginBottom: 6 },
  cardFooter: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  dateText: { fontSize: 11, fontFamily: 'regular' },
  pendingBadge: {
    backgroundColor: 'rgba(255,152,0,0.12)',
    borderRadius: 8,
    paddingHorizontal: 8,
    paddingVertical: 3,
    borderWidth: 1,
    borderColor: 'rgba(255,152,0,0.35)',
  },
  pendingText: { fontSize: 11, fontFamily: 'semiBold', color: '#E65100' },

  /* Empty / Guest state */
  centerState: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 32,
  },
  guestIconBox: {
    width: 72,
    height: 72,
    borderRadius: 18,
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 20,
  },
  emptyTitle: { fontSize: 20, fontFamily: 'bold', textAlign: 'center', marginBottom: 8 },
  emptySubtitle: {
    fontSize: 14,
    fontFamily: 'regular',
    color: 'gray',
    textAlign: 'center',
    lineHeight: 20,
    marginBottom: 24,
  },
  loginBtn: {
    backgroundColor: COLORS.primary,
    paddingHorizontal: 36,
    paddingVertical: 13,
    borderRadius: 30,
  },
  loginBtnText: { fontSize: 15, fontFamily: 'semiBold', color: '#FFFFFF' },
});

export default MyInquiries;
