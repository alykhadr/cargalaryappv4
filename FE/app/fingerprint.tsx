import { useNavigation } from 'expo-router';
import React, { useMemo, useState } from 'react';
import Text from '@/components/LocalizedText';
import { Alert, Image, ScrollView, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Button from '../components/Button';
import ButtonFilled from '../components/ButtonFilled';
import Header from '../components/Header';
import { COLORS, SIZES, illustrations } from '../constants';
import { useTheme } from '../theme/ThemeProvider';
import { useAuth } from '../context/AuthContext';

type Nav = {
  navigate: (value: string) => void;
  goBack: () => void;
};

const Fingerprint = () => {
  const navigation = useNavigation<Nav>();
  const { colors, dark } = useTheme();
  const { biometricCapabilities, setBiometricPreference } = useAuth();
  const [isSaving, setIsSaving] = useState(false);

  const setupLabel = useMemo(() => {
    if (biometricCapabilities.supportsFaceId) {
      return 'Face ID';
    }

    if (biometricCapabilities.supportsFingerprint) {
      return 'Fingerprint';
    }

    return 'Biometrics';
  }, [biometricCapabilities]);

  const handleContinue = async () => {
    setIsSaving(true);
    try {
      if (biometricCapabilities.supportsFaceId) {
        const result = await setBiometricPreference('faceIdEnabled', true);
        if (!result.enabled) {
          Alert.alert('Unable to enable Face ID', result.message || 'Please try again.');
          return;
        }
      } else if (biometricCapabilities.supportsFingerprint) {
        const result = await setBiometricPreference('fingerprintEnabled', true);
        if (!result.enabled) {
          Alert.alert('Unable to enable Fingerprint', result.message || 'Please try again.');
          return;
        }
      } else {
        Alert.alert('Not available', 'No biometric method is available on this device.');
        return;
      }

      Alert.alert('Security Updated', `${setupLabel} login has been enabled.`, [
        {
          text: 'Continue',
          onPress: () => navigation.goBack(),
        },
      ]);
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title={`Set Your ${setupLabel}`} />
        <ScrollView contentContainerStyle={styles.scrollContent} showsVerticalScrollIndicator={false}>
          <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
            Add {setupLabel} to make your account more secure.
          </Text>
          <Image
            source={dark ? illustrations.fingerprintDark : illustrations.fingerprint}
            resizeMode="contain"
            style={styles.fingerprint}
          />
          <Text style={[styles.subtitle, { color: dark ? COLORS.grayscale200 : COLORS.grayscale700 }]}>
            We&apos;ll ask you to confirm with {setupLabel.toLowerCase()} before unlocking your saved session.
          </Text>
        </ScrollView>
      </View>

      <View style={styles.bottomContainer}>
        <Button
          title="Skip"
          style={{
            width: (SIZES.width - 32) / 2 - 8,
            borderRadius: 32,
            backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
            borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
          }}
          textColor={dark ? COLORS.white : COLORS.black}
          onPress={() => navigation.goBack()}
        />
        <ButtonFilled
          title={isSaving ? 'Enabling...' : 'Enable'}
          style={styles.continueButton}
          onPress={handleContinue}
          isLoading={isSaving}
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
  scrollContent: {
    alignItems: 'center',
    paddingBottom: 140,
  },
  title: {
    fontSize: 18,
    fontFamily: 'medium',
    textAlign: 'center',
    marginVertical: 36,
  },
  subtitle: {
    fontSize: 16,
    fontFamily: 'regular',
    textAlign: 'center',
    lineHeight: 22,
    marginTop: 24,
  },
  fingerprint: {
    width: 300,
    height: 300,
  },
  bottomContainer: {
    position: 'absolute',
    bottom: 32,
    right: 16,
    left: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: SIZES.width - 32,
    alignItems: 'center',
  },
  continueButton: {
    width: (SIZES.width - 32) / 2 - 8,
    borderRadius: 32,
    backgroundColor: COLORS.primary,
    borderColor: COLORS.primary,
  },
});

export default Fingerprint;
