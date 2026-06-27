import { FONTS } from '@/constants/fonts';
import { DirectionProvider, useDirection } from '@/theme/DirectionProvider';
import { ThemeProvider } from '@/theme/ThemeProvider';
import { useFonts } from 'expo-font';
import { Stack } from 'expo-router';
import * as SplashScreen from 'expo-splash-screen';
import { useEffect } from 'react';
import { StyleSheet, View } from 'react-native';
import { AuthProvider, useAuth } from '../context/AuthContext';

// Prevent the splash screen from auto-hiding before asset loading is complete.
SplashScreen.preventAutoHideAsync();


function AppStack() {
  const { isRTL, rootDirectionStyle } = useDirection();

  return (
    <View style={[styles.root, rootDirectionStyle]}>
      <Stack screenOptions={{
        headerShown: false,
        animation: isRTL ? 'slide_from_left' : 'slide_from_right',
      }}>
        <Stack.Screen name="index" />
        <Stack.Screen name="addnewaddress" />
        <Stack.Screen name="addnewcard" />
        <Stack.Screen name="addpromo" />
        <Stack.Screen name="address" />
        <Stack.Screen name="call" />
        <Stack.Screen name="cancelorder" />
        <Stack.Screen name="cancelorderpaymentmethods" />
        <Stack.Screen name="categories" />
        <Stack.Screen name="category/[id]" />
        <Stack.Screen name="changeemail" />
        <Stack.Screen name="changepassword" />
        <Stack.Screen name="changepin" />
        <Stack.Screen name="chat" />
        <Stack.Screen name="checkout" />
        <Stack.Screen name="checkoutsuccessful" />
        <Stack.Screen name="chooseshippingmethods" />
        <Stack.Screen name="createnewpin" />
        <Stack.Screen name="createnewpassword" />
        <Stack.Screen name="customerservice" />
        <Stack.Screen name="editprofile" />
        <Stack.Screen name="enteryourpin" />
        <Stack.Screen name="ereceipt" />
        <Stack.Screen name="fillyourprofile" />
        <Stack.Screen name="fingerprint" />
        <Stack.Screen name="forgotpasswordemail" />
        <Stack.Screen name="forgotpasswordmethods" />
        <Stack.Screen name="forgotpasswordphonenumber" />
        <Stack.Screen name="login" />
        <Stack.Screen name="mostpopularproducts" />
        <Stack.Screen name="mywishlist" />
        <Stack.Screen name="onboarding" />
        <Stack.Screen name="otpverification" />
        <Stack.Screen name="paymentmethods" />
        <Stack.Screen name="productereceipt" />
        <Stack.Screen name="productreviews" />
        <Stack.Screen name="search" />
        <Stack.Screen name="selectshippingaddress" />
        <Stack.Screen name="settingshelpcenter" />
        <Stack.Screen name="settingsinvitefriends" />
        <Stack.Screen name="settingslanguage" />
        <Stack.Screen name="settingsnotifications" />
        <Stack.Screen name="settingspayment" />
        <Stack.Screen name="settingsprivacypolicy" />
        <Stack.Screen name="settingssecurity" />
        <Stack.Screen name="signup" />
        <Stack.Screen name="topupereceipt" />
        <Stack.Screen name="topupewalletamount" />
        <Stack.Screen name="topupewalletmethods" />
        <Stack.Screen name="trackorder" />
        <Stack.Screen name="transactionhistory" />
        <Stack.Screen name="videocall" />
        <Stack.Screen name="cardetails" />
        <Stack.Screen name="makeoffer" />
        <Stack.Screen name="makeofferprocessed" />
        <Stack.Screen name="welcome" />
        <Stack.Screen name="(tabs)" />
      </Stack>
    </View>
  );
}

export default function RootLayout() {
  const [loaded] = useFonts(FONTS);

  useEffect(() => {
    if (loaded) {
      SplashScreen.hideAsync();
    }
  }, [loaded]);

  if (!loaded) {
    return null;
  }

  return (
    <ThemeProvider>
      <DirectionProvider>
        <AuthProvider>
          <AppStack />
        </AuthProvider>
      </DirectionProvider>
    </ThemeProvider>
  );
}

const styles = StyleSheet.create({
  root: {
    flex: 1,
  },
});
