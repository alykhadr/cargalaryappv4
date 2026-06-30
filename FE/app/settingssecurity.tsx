import { View, StyleSheet, Alert } from 'react-native';
import React, { useEffect } from 'react';
import Text from '@/components/LocalizedText';
import { COLORS } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { ScrollView } from 'react-native-virtualized-view';
import GlobalSettingsItem from '../components/GlobalSettingsItem';
import Button from '../components/Button';
import { useTheme } from '../theme/ThemeProvider';
import { useNavigation } from 'expo-router';
import { useAuth } from '../context/AuthContext';

type Nav = {
  navigate: (value: string) => void;
};

const SettingsSecurity = () => {
  const { navigate } = useNavigation<Nav>();
  const { colors, dark } = useTheme();
  const {
    biometricSettings,
    biometricCapabilities,
    refreshBiometricCapabilities,
    setBiometricPreference,
  } = useAuth();

  useEffect(() => {
    refreshBiometricCapabilities().catch(() => {});
  }, [refreshBiometricCapabilities]);

  const toggleBiometric = async (key: 'faceIdEnabled' | 'fingerprintEnabled', nextValue: boolean) => {
    const result = await setBiometricPreference(key, nextValue);
    if (result.message) {
      Alert.alert(nextValue ? 'Security Updated' : 'Security Disabled', result.message);
    }
  };

  const securityNote = !biometricCapabilities.hasHardware
    ? 'Biometric authentication is not supported on this device.'
    : !biometricCapabilities.isEnrolled
      ? 'Add a fingerprint or face unlock in your device settings first.'
      : 'Enable the biometric option you want to use for faster login.';

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Security" />
        <ScrollView style={styles.scrollView} showsVerticalScrollIndicator={false}>
          <Text style={[styles.note, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            {securityNote}
          </Text>

          <GlobalSettingsItem
            title="Face ID"
            isNotificationEnabled={biometricSettings.faceIdEnabled}
            toggleNotificationEnabled={() =>
              toggleBiometric('faceIdEnabled', !biometricSettings.faceIdEnabled)
            }
          />
          <Text style={[styles.helperText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            {biometricCapabilities.supportsFaceId
              ? 'Use facial recognition to unlock your saved session.'
              : 'Face ID is not available on this device.'}
          </Text>

          <GlobalSettingsItem
            title="Fingerprint"
            isNotificationEnabled={biometricSettings.fingerprintEnabled}
            toggleNotificationEnabled={() =>
              toggleBiometric('fingerprintEnabled', !biometricSettings.fingerprintEnabled)
            }
          />
          <Text style={[styles.helperText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            {biometricCapabilities.supportsFingerprint
              ? 'Use your fingerprint to log in without typing your password.'
              : 'Fingerprint authentication is not available on this device.'}
          </Text>

          <Button
            title="Change Password"
            style={[
              styles.actionButton,
              {
                backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
                borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
              },
            ]}
            textColor={dark ? COLORS.white : COLORS.black}
            onPress={() => navigate('changepassword')}
          />
        </ScrollView>
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
  scrollView: {
    marginVertical: 22,
  },
  note: {
    fontSize: 14,
    fontFamily: 'regular',
    lineHeight: 20,
    marginBottom: 8,
  },
  helperText: {
    fontSize: 13,
    fontFamily: 'regular',
    marginTop: -4,
    marginBottom: 12,
    lineHeight: 18,
  },
  actionButton: {
    borderRadius: 32,
    marginTop: 22,
  },
});

export default SettingsSecurity;
