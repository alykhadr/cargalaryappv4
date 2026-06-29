import { View, StyleSheet, TouchableOpacity } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, SIZES } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { useTheme } from '../theme/ThemeProvider';
import ButtonFilled from '../components/ButtonFilled';
import UserAddressItem from '@/components/UserAddressItem';
import { useNavigation } from 'expo-router';
import { useAuth } from '@/context/AuthContext';

type Nav = {
  navigate: (value: string) => void;
};

const Address = () => {
  const { navigate } = useNavigation<Nav>();
  const { colors, dark } = useTheme();
  const { user, isGuest } = useAuth();

  const savedAddress = user?.country?.trim() || '';

  if (isGuest || !user) {
    return (
      <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
        <View style={[styles.container, { backgroundColor: colors.background }]}>
          <Header title="Address" />
          <View style={styles.emptyWrap}>
            <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
              Login to manage your address
            </Text>
            <Text style={[styles.emptySubtitle, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
              Your saved address is linked to your profile.
            </Text>
            <TouchableOpacity style={styles.loginBtn} onPress={() => navigate('login')}>
              <Text style={styles.loginBtnText}>Login</Text>
            </TouchableOpacity>
          </View>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Address" />
        <View style={styles.content}>
          {savedAddress ? (
            <UserAddressItem
              name="Saved Address"
              address={savedAddress}
              onPress={() => navigate('addnewaddress')}
            />
          ) : (
            <View style={styles.emptyWrap}>
              <Text style={[styles.emptyTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                No address saved yet
              </Text>
              <Text style={[styles.emptySubtitle, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
                Add one address to your profile and we&apos;ll show it here.
              </Text>
            </View>
          )}
        </View>
      </View>
      <View style={styles.btnContainer}>
        <ButtonFilled
          title={savedAddress ? 'Update Address' : 'Add Address'}
          onPress={() => navigate('addnewaddress')}
          style={styles.btn}
        />
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
  content: {
    flex: 1,
    marginTop: 12,
  },
  btnContainer: {
    alignItems: 'center',
    paddingBottom: 20,
  },
  btn: {
    width: SIZES.width - 32,
    paddingHorizontal: 16,
  },
  emptyWrap: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 28,
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
    lineHeight: 20,
  },
  loginBtn: {
    backgroundColor: COLORS.primary,
    paddingHorizontal: 36,
    paddingVertical: 13,
    borderRadius: 30,
    marginTop: 22,
  },
  loginBtnText: {
    color: COLORS.white,
    fontSize: 16,
    fontFamily: 'semiBold',
  },
});

export default Address;
