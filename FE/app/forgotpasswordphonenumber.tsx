import { useNavigation } from 'expo-router';
import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import { Alert, Image, ScrollView, StyleSheet, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import ButtonFilled from '../components/ButtonFilled';
import Header from '../components/Header';
import Input from '../components/Input';
import { COLORS, SIZES, icons, images } from '../constants';
import { api } from '../services/api';
import { useTheme } from '../theme/ThemeProvider';

type Nav = {
  navigate: (value: string) => void;
};

const ForgotPasswordUsername = () => {
  const { navigate } = useNavigation<Nav>();
  const [userName, setUserName] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const { colors, dark } = useTheme();

  const handleResetPassword = async () => {
    const trimmedUserName = userName.trim();
    if (!trimmedUserName) {
      Alert.alert('Error', 'Please enter your username');
      return;
    }

    setIsLoading(true);
    try {
      const response = await api.forgotPassword({ userNameOrEmail: trimmedUserName });
      Alert.alert('Reset Link Sent', response.message || 'Check your email for the reset link.');
      navigate('login');
    } catch (error: any) {
      Alert.alert('Request Failed', error?.message || 'Unable to send password reset link');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Forgot Password" />
        <ScrollView style={{ marginVertical: 54 }} showsVerticalScrollIndicator={false}>
          <View style={styles.logoContainer}>
            <Image
              source={images.logo}
              resizeMode="contain"
              style={[styles.logo, { tintColor: dark ? COLORS.white : COLORS.black }]}
            />
          </View>
          <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.black }]}>
            Enter Your Username
          </Text>
          <Text style={[styles.subtitle, { color: dark ? COLORS.grayscale200 : COLORS.greyscale600 }]}>
            We&apos;ll look up your account and send the reset link to the email connected to that username.
          </Text>
          <Input
            id="userName"
            value={userName}
            onInputChanged={(_, value) => setUserName(value)}
            placeholder="Username"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
            icon={icons.user}
            autoCapitalize="none"
          />
          <ButtonFilled
            title={isLoading ? 'Sending...' : 'Send Reset Link'}
            onPress={handleResetPassword}
            style={styles.button}
          />
          <TouchableOpacity onPress={() => navigate('login')}>
            <Text style={[styles.loginLink, { color: dark ? COLORS.white : COLORS.primary }]}>
              Remembered your password?
            </Text>
          </TouchableOpacity>
        </ScrollView>
        <View style={styles.bottomContainer}>
          <Text style={[styles.bottomLeft, { color: dark ? COLORS.white : COLORS.black }]}>
            Don&apos;t have an account ?
          </Text>
          <TouchableOpacity onPress={() => navigate('signup')}>
            <Text style={[styles.bottomRight, { color: dark ? COLORS.white : COLORS.primary }]}>
              {'  '}Sign Up
            </Text>
          </TouchableOpacity>
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
  logo: {
    width: 100,
    height: 100,
    tintColor: COLORS.primary,
  },
  logoContainer: {
    alignItems: 'center',
    justifyContent: 'center',
    marginVertical: 32,
  },
  title: {
    fontSize: 26,
    fontFamily: 'bold',
    color: COLORS.black,
    textAlign: 'center',
    marginBottom: 12,
  },
  subtitle: {
    fontSize: 14,
    fontFamily: 'regular',
    textAlign: 'center',
    marginBottom: 22,
    lineHeight: 20,
  },
  bottomContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    marginVertical: 18,
    position: 'absolute',
    bottom: 12,
    right: 0,
    left: 0,
  },
  bottomLeft: {
    fontSize: 14,
    fontFamily: 'regular',
    color: 'black',
  },
  bottomRight: {
    fontSize: 16,
    fontFamily: 'medium',
    color: COLORS.primary,
  },
  button: {
    marginVertical: 6,
    width: SIZES.width - 32,
    borderRadius: 30,
  },
  loginLink: {
    fontSize: 16,
    fontFamily: 'semiBold',
    textAlign: 'center',
    marginTop: 12,
  },
});

export default ForgotPasswordUsername;
