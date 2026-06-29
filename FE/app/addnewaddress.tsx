import ButtonFilled from '@/components/ButtonFilled';
import { useNavigation } from 'expo-router';
import React, { useEffect, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { Alert, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { COLORS, SIZES } from '../constants';
import { useAuth } from '@/context/AuthContext';
import { useTheme } from '../theme/ThemeProvider';

type Nav = {
  goBack: () => void;
};

const AddNewAddress = () => {
  const navigation = useNavigation<Nav>();
  const { dark, colors } = useTheme();
  const { user, isGuest, updateProfile } = useAuth();
  const [address, setAddress] = useState(user?.country ?? '');
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    setAddress(user?.country ?? '');
  }, [user?.country]);

  const handleSave = async () => {
    const trimmedAddress = address.trim();
    if (!trimmedAddress) {
      Alert.alert('Error', 'Please enter your address');
      return;
    }

    setIsSaving(true);
    try {
      await updateProfile({ country: trimmedAddress });
      Alert.alert('Address Updated', 'Your profile address has been saved.');
      navigation.goBack();
    } catch (error: any) {
      Alert.alert('Save Failed', error?.message || 'Unable to save address');
    } finally {
      setIsSaving(false);
    }
  };

  if (isGuest || !user) {
    return (
      <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
        <View style={[styles.container, { backgroundColor: colors.background }]}>
          <Header title="Add Address" />
          <View style={styles.centerWrap}>
            <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
              Login required
            </Text>
            <Text style={[styles.subtitle, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
              Address is stored on your profile, so you need to be signed in to edit it.
            </Text>
          </View>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title={user?.country ? 'Update Address' : 'Add Address'} />
        <View style={styles.formWrap}>
          <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
            Saved Address
          </Text>
          <Text style={[styles.subtitle, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            This is the address currently tied to your account and visible from the profile address screen.
          </Text>
          <TextInput
            value={address}
            onChangeText={setAddress}
            placeholder="Enter your address"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.grayscale700}
            multiline
            textAlignVertical="top"
            style={[
              styles.addressInput,
              {
                color: dark ? COLORS.white : COLORS.greyscale900,
                backgroundColor: dark ? COLORS.dark2 : COLORS.grayscale100,
                borderColor: dark ? COLORS.dark3 : COLORS.grayscale200,
              },
            ]}
          />
        </View>
        <View style={styles.buttonWrap}>
          <ButtonFilled
            title={isSaving ? 'Saving...' : 'Save Address'}
            onPress={handleSave}
            style={styles.button}
          />
        </View>
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
    padding: 16,
    backgroundColor: COLORS.white,
  },
  formWrap: {
    flex: 1,
    marginTop: 18,
  },
  centerWrap: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: 24,
  },
  title: {
    fontSize: 20,
    fontFamily: 'bold',
    marginBottom: 10,
  },
  subtitle: {
    fontSize: 14,
    fontFamily: 'regular',
    lineHeight: 20,
    marginBottom: 18,
  },
  addressInput: {
    minHeight: 160,
    borderRadius: 16,
    borderWidth: 1,
    paddingHorizontal: 16,
    paddingVertical: 14,
    fontSize: 15,
    fontFamily: 'regular',
  },
  buttonWrap: {
    paddingBottom: 20,
  },
  button: {
    width: SIZES.width - 32,
    borderRadius: 30,
  },
});

export default AddNewAddress;
