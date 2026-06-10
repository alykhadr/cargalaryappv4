import { View, StyleSheet, TouchableOpacity, Image, Switch } from 'react-native';
import React, { useState, useRef } from 'react';
import AppAvatar from '@/components/AppAvatar';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { MaterialIcons } from '@expo/vector-icons';
import { useDirection } from '@/theme/DirectionProvider';
import { useTheme } from '@/theme/ThemeProvider';
import { COLORS, icons, images, SIZES } from '@/constants';
import { launchImagePicker } from '@/utils/ImagePickerHelper';
import SettingsItem from '@/components/SettingsItem';
import { useNavigation } from 'expo-router';
import Button from '@/components/Button';
import ButtonFilled from '@/components/ButtonFilled';
import RBSheet from "react-native-raw-bottom-sheet";
import { useAuth } from '@/context/AuthContext';

type Nav = {
  navigate: (value: string) => void
}

const Profile = () => {
  const refRBSheet = useRef<any>(null);
  const [photoUri, setPhotoUri] = useState<string | null>(null);
  const { dark, colors, setScheme } = useTheme();
  const { direction, iconFlipStyle, language, textDirectionStyle } = useDirection();
  const { navigate } = useNavigation<Nav>();
  const { user, isGuest, logout } = useAuth();

  const handleLogout = async () => {
    await logout();
    refRBSheet.current?.close();
    navigate('login');
  };
  /**
   * Render header
   */
  const renderHeader = () => {
    return (
      <TouchableOpacity style={styles.headerContainer}>
        <View style={styles.headerLeft}>
          <Text style={[styles.headerTitle, textDirectionStyle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Profile</Text>
        </View>
        <TouchableOpacity>
          <Image
            source={icons.moreCircle}
            resizeMode='contain'
            style={[styles.headerIcon, {
              tintColor: dark ? COLORS.secondaryWhite : COLORS.greyscale900
            }]}
          />
        </TouchableOpacity>
      </TouchableOpacity>
    )
  }
  /**
   * Render User Profile
   */
  const renderProfile = () => {
    const pickImage = async () => {
      try {
        const tempUri = await launchImagePicker();
        if (!tempUri) return;
        setPhotoUri(tempUri);
      } catch {}
    };

    return (
      <View style={[styles.profileContainer, {
        borderBottomColor: dark ? 'rgba(255,255,255,0.10)' : COLORS.grayscale400,
      }]}>
        <View style={styles.avatarWrap}>
          <AppAvatar uri={photoUri ?? user?.avatarUrl ?? null} size={110} />
          {!isGuest && (
            <TouchableOpacity onPress={pickImage} style={styles.picContainer}>
              <MaterialIcons name="edit" size={16} color={COLORS.white} />
            </TouchableOpacity>
          )}
        </View>
        <Text style={[styles.title, { color: dark ? '#FFFFFF' : COLORS.greyscale900 }]}>
          {isGuest ? 'Guest User' : (user?.fullName || 'User')}
        </Text>
        <Text style={[styles.subtitle, { color: dark ? 'rgba(255,255,255,0.55)' : COLORS.gray }]}>
          {isGuest ? 'Login to access your account' : (user?.email || '')}
        </Text>
      </View>
    )
  }
  /**
   * Render Settings
   */
  const renderSettings = () => {
    const toggleDarkMode = () => {
      setScheme(dark ? 'light' : 'dark');
    };

    return (
      <View style={styles.settingsContainer}>
        {!isGuest && (
          <>
            <SettingsItem
              icon={icons.bell3}
              name="My Notification"
              onPress={() => navigate("notifications")}
            />
            <SettingsItem
              icon={icons.location2Outline}
              name="Address"
              onPress={() => navigate("address")}
            />
            <SettingsItem
              icon={icons.userOutline}
              name="Edit Profile"
              onPress={() => navigate("editprofile")}
            />
            <SettingsItem
              icon={icons.shieldOutline}
              name="Security"
              onPress={() => navigate("settingssecurity")}
            />
          </>
        )}
        <TouchableOpacity
          onPress={() => navigate("settingslanguage")}
          style={styles.settingsItemContainer}>
          <View style={styles.leftContainer}>
            <Image
              source={icons.more}
              resizeMode='contain'
              style={[styles.settingsIcon, {
                tintColor: dark ? COLORS.white : COLORS.greyscale900
              }]}
            />
            <Text style={[styles.settingsName, textDirectionStyle, {
              color: dark ? COLORS.white : COLORS.greyscale900
            }]}>Language & Region</Text>
          </View>
          <View style={styles.rightContainer}>
            <Text style={[styles.rightLanguage, textDirectionStyle, {
              color: dark ? COLORS.white : COLORS.greyscale900
            }]}>{language} - {direction.toUpperCase()}</Text>
            <Image
              source={icons.arrowRight}
              resizeMode='contain'
              style={[styles.settingsArrowRight, {
                tintColor: dark ? COLORS.white : COLORS.greyscale900
              }, iconFlipStyle]}
            />
          </View>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.settingsItemContainer}>
          <View style={styles.leftContainer}>
            <Image
              source={icons.show}
              resizeMode='contain'
              style={[styles.settingsIcon, {
                tintColor: dark ? COLORS.white : COLORS.greyscale900
              }]}
            />
            <Text style={[styles.settingsName, textDirectionStyle, {
              color: dark ? COLORS.white : COLORS.greyscale900
            }]}>Dark Mode</Text>
          </View>
          <View style={styles.rightContainer}>
            <Switch
              value={dark}
              onValueChange={toggleDarkMode}
              thumbColor="#FFFFFF"
              trackColor={{ false: '#CCCCCC', true: '#E8001C' }}
              ios_backgroundColor="#CCCCCC"
              style={styles.switch}
            />
          </View>
        </TouchableOpacity>
        <SettingsItem
          icon={icons.lockedComputerOutline}
          name="Privacy Policy"
          onPress={() => navigate("settingsprivacypolicy")}
        />
        <SettingsItem
          icon={icons.infoCircle}
          name="Help Center"
          onPress={() => navigate("settingshelpcenter")}
        />
        {isGuest ? (
          <TouchableOpacity
            onPress={() => navigate("login")}
            style={styles.loginCTABtn}
            activeOpacity={0.85}
          >
            <Text style={styles.loginCTAText}>Login / Sign Up</Text>
          </TouchableOpacity>
        ) : (
          <TouchableOpacity
            onPress={() => refRBSheet.current.open()}
            style={styles.logoutContainer}>
            <View style={styles.logoutLeftContainer}>
              <Image
                source={icons.logout}
                resizeMode='contain'
                style={[styles.logoutIcon, { tintColor: 'red' }]}
              />
              <Text style={[styles.logoutName, textDirectionStyle, { color: 'red' }]}>
                Logout
              </Text>
            </View>
          </TouchableOpacity>
        )}
      </View>
    )
  }
  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <ScrollView showsVerticalScrollIndicator={false}>
          {renderProfile()}
          {renderSettings()}
        </ScrollView>
      </View>
      <RBSheet
        ref={refRBSheet}
        closeOnPressMask={true}
        height={SIZES.height * .8}
        customStyles={{
          wrapper: {
            backgroundColor: "rgba(0,0,0,0.5)",
          },
          draggableIcon: {
            backgroundColor: dark ? COLORS.gray2 : COLORS.grayscale200,
            height: 4
          },
          container: {
            borderTopRightRadius: 32,
            borderTopLeftRadius: 32,
            height: 260,
            backgroundColor: dark ? COLORS.dark2 : COLORS.white
          }
        }}
      >
        <Text style={styles.bottomTitle}>Logout</Text>
        <View style={[styles.separateLine, {
          backgroundColor: dark ? COLORS.greyScale800 : COLORS.grayscale200,
        }]} />
        <Text style={[styles.bottomSubtitle, {
          color: dark ? COLORS.white : COLORS.black
        }]}>Are you sure you want to log out?</Text>
        <View style={styles.bottomContainer}>
          <Button
            title="Cancel"
            style={{
              width: (SIZES.width - 32) / 2 - 8,
              backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
              borderRadius: 32,
              borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary
            }}
            textColor={dark ? COLORS.white : COLORS.primary}
            onPress={() => refRBSheet.current.close()}
          />
          <ButtonFilled
            title="Yes, Logout"
            style={styles.logoutButton}
            onPress={handleLogout}
          />
        </View>
      </RBSheet>
    </SafeAreaView>
  )
};

const styles = StyleSheet.create({
  area: {
    flex: 1,
    backgroundColor: COLORS.white
  },
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
    padding: 16,
    marginBottom: 32
  },
  headerContainer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between"
  },
  headerLeft: {
    flexDirection: "row",
    alignItems: "center"
  },
  logo: {
    height: 32,
    width: 32,
    tintColor: COLORS.primary
  },
  headerTitle: {
    fontSize: 22,
    fontFamily: "bold",
    color: COLORS.greyscale900,
    marginStart: 12
  },
  headerIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.greyscale900
  },
  profileContainer: {
    alignItems: 'center',
    borderBottomWidth: 0.5,
    paddingVertical: 20,
  },
  loginCTABtn: {
    width: SIZES.width - 32,
    height: 52,
    backgroundColor: '#E8001C',
    borderRadius: 30,
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: 16,
    marginBottom: 8,
    shadowColor: '#E8001C',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.40,
    shadowRadius: 12,
    elevation: 6,
  },
  loginCTAText: {
    fontSize: 16,
    fontFamily: 'bold',
    color: '#FFFFFF',
    letterSpacing: 0.3,
  },
  avatarWrap: {
    position: 'relative',
    width: 110,
    height: 110,
    shadowColor: '#E8001C',
    shadowOffset: { width: 0, height: 6 },
    shadowOpacity: 0.30,
    shadowRadius: 12,
    elevation: 8,
  },
  picContainer: {
    width: 30,
    height: 30,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: COLORS.primary,
    position: 'absolute',
    right: -4,
    bottom: -4,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 4,
  },
  title: {
    fontSize: 18,
    fontFamily: "bold",
    color: COLORS.greyscale900,
    marginTop: 12
  },
  subtitle: {
    fontSize: 16,
    color: COLORS.greyscale900,
    fontFamily: "medium",
    marginTop: 4
  },
  settingsContainer: {
    marginVertical: 12
  },
  settingsItemContainer: {
    width: SIZES.width - 32,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginVertical: 12
  },
  leftContainer: {
    flexDirection: "row",
    alignItems: "center",
  },
  settingsIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.greyscale900
  },
  settingsName: {
    fontSize: 18,
    fontFamily: "semiBold",
    color: COLORS.greyscale900,
    marginStart: 12
  },
  settingsArrowRight: {
    width: 24,
    height: 24,
    tintColor: COLORS.greyscale900
  },
  rightContainer: {
    flexDirection: "row",
    alignItems: "center"
  },
  rightLanguage: {
    fontSize: 18,
    fontFamily: "semiBold",
    color: COLORS.greyscale900,
    marginEnd: 8
  },
  switch: {
    marginStart: 8,
    transform: [{ scaleX: .8 }, { scaleY: .8 }], // Adjust the size of the switch
  },
  logoutContainer: {
    width: SIZES.width - 32,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginVertical: 12
  },
  logoutLeftContainer: {
    flexDirection: "row",
    alignItems: "center",
  },
  logoutIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.greyscale900
  },
  logoutName: {
    fontSize: 18,
    fontFamily: "semiBold",
    color: COLORS.greyscale900,
    marginStart: 12
  },
  bottomContainer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginVertical: 12,
    paddingHorizontal: 16
  },
  cancelButton: {
    width: (SIZES.width - 32) / 2 - 8,
    backgroundColor: COLORS.tansparentPrimary,
    borderRadius: 32
  },
  logoutButton: {
    width: (SIZES.width - 32) / 2 - 8,
    backgroundColor: COLORS.primary,
    borderRadius: 32
  },
  bottomTitle: {
    fontSize: 24,
    fontFamily: "semiBold",
    color: "red",
    textAlign: "center",
    marginTop: 12
  },
  bottomSubtitle: {
    fontSize: 20,
    fontFamily: "semiBold",
    color: COLORS.greyscale900,
    textAlign: "center",
    marginVertical: 28
  },
  separateLine: {
    width: SIZES.width,
    height: 1,
    backgroundColor: COLORS.grayscale200,
    marginTop: 12
  }
})

export default Profile
