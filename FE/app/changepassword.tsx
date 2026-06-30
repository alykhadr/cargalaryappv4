import { View, StyleSheet, ScrollView, Alert } from 'react-native';
import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { COLORS, SIZES, icons } from '../constants';
import Header from '../components/Header';
import Input from '../components/Input';
import ButtonFilled from '../components/ButtonFilled';
import { useTheme } from '../theme/ThemeProvider';
import { api } from '../services/api';
import { useNavigation } from 'expo-router';

type Nav = {
  goBack: () => void;
};

const ChangePassword = () => {
  const { colors, dark } = useTheme();
  const navigation = useNavigation<Nav>();
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async () => {
    if (!currentPassword.trim() || !newPassword.trim() || !confirmPassword.trim()) {
      Alert.alert('Missing details', 'Please fill all password fields.');
      return;
    }

    if (newPassword.length < 6) {
      Alert.alert('Weak password', 'Your new password must be at least 6 characters long.');
      return;
    }

    if (newPassword !== confirmPassword) {
      Alert.alert('Password mismatch', 'The new password and confirmation do not match.');
      return;
    }

    setIsLoading(true);
    try {
      const response = await api.changePassword({
        currentPassword: currentPassword.trim(),
        newPassword: newPassword.trim(),
      });

      Alert.alert('Password Updated', response.message || 'Your password has been changed successfully.', [
        {
          text: 'OK',
          onPress: () => navigation.goBack(),
        },
      ]);
    } catch (error: any) {
      Alert.alert('Unable to update password', error?.message || 'Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Change Password" />
        <ScrollView showsVerticalScrollIndicator={false} contentContainerStyle={styles.content}>
          <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.black }]}>
            Keep your account secure
          </Text>
          <Text style={[styles.subtitle, { color: dark ? COLORS.grayscale200 : COLORS.grayscale700 }]}>
            Update your password here. If biometric login is enabled, it will keep working with your saved session.
          </Text>

          <Input
            id="currentPassword"
            value={currentPassword}
            onInputChanged={(_, value) => setCurrentPassword(value)}
            placeholder="Current Password"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
            icon={icons.padlock}
            secureTextEntry
          />
          <Input
            id="newPassword"
            value={newPassword}
            onInputChanged={(_, value) => setNewPassword(value)}
            placeholder="New Password"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
            icon={icons.padlock}
            secureTextEntry
          />
          <Input
            id="confirmPassword"
            value={confirmPassword}
            onInputChanged={(_, value) => setConfirmPassword(value)}
            placeholder="Confirm New Password"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
            icon={icons.padlock}
            secureTextEntry
          />
        </ScrollView>

        <ButtonFilled
          title={isLoading ? 'Updating...' : 'Update Password'}
          onPress={handleSubmit}
          style={styles.button}
          isLoading={isLoading}
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
    padding: 16,
    backgroundColor: COLORS.white,
  },
  content: {
    paddingTop: 24,
    paddingBottom: 24,
  },
  title: {
    fontSize: 22,
    fontFamily: 'bold',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 14,
    fontFamily: 'regular',
    lineHeight: 20,
    marginBottom: 18,
  },
  button: {
    marginVertical: 6,
    width: SIZES.width - 32,
    borderRadius: 30,
  },
});

export default ChangePassword;
