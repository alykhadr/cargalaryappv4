import { Ionicons } from '@expo/vector-icons';
import { NavigationProp } from '@react-navigation/native';
import { useNavigation } from 'expo-router';
import React from 'react';
import Text from '@/components/LocalizedText';
import { Image, StyleSheet, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { COLORS, icons } from '../constants';
import { useTheme } from '../theme/ThemeProvider';

type NotificationItemProps = {
  icon: keyof typeof Ionicons.glyphMap;
  title: string;
  description: string;
};

const Notifications = () => {
  const { colors, dark } = useTheme();
  const navigation = useNavigation<NavigationProp<any>>();

  /**
   * Render header
   */
  const renderHeader = () => {
    return (
      <View style={styles.headerContainer}>
        <TouchableOpacity
          onPress={() => navigation.goBack()}
          style={[styles.headerIconContainer, {
            borderColor: dark ? COLORS.dark3 : COLORS.grayscale200
          }]}>
          <Image
            source={icons.back}
            resizeMode='contain'
            style={[styles.arrowBackIcon, {
              tintColor: dark ? COLORS.white : COLORS.greyscale900
            }]}
          />
        </TouchableOpacity>
        <Text style={[styles.headerTitle, {
          color: dark ? COLORS.white : COLORS.greyscale900
        }]}>Notifications</Text>
        <Text>{"  "}</Text>
      </View>
    )
  }

  const NotificationItem: React.FC<NotificationItemProps> = ({ icon, title, description }) => {
    return (
      <View style={[styles.notificationItem, {
        backgroundColor: dark ? COLORS.dark2 : '#F5F5F5',
      }]}>
        <View style={[styles.iconContainer, {
          backgroundColor: dark ? COLORS.white : '#000',
        }]}>
          <Ionicons name={icon} size={24} color={dark ? COLORS.greyscale900 : "white"} />
        </View>
        <View>
          <Text style={[styles.notificationTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900,
          }]}>{title}</Text>
          <Text style={[styles.notificationDescription, {
            color: dark ? COLORS.grayscale400 : 'gray',
          }]}>{description}</Text>
        </View>
      </View>
    );
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <ScrollView showsVerticalScrollIndicator={false}>
          <Text style={[styles.sectionTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>Today</Text>
          <NotificationItem
            icon="pricetag"
            title="30% Special Discount!"
            description="Special promotion only valid today"
          />

          <Text style={[styles.sectionTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>Yesterday</Text>
          <NotificationItem
            icon="wallet"
            title="Top Up E-Wallet Successful!"
            description="You have to top up your e-wallet"
          />
          <NotificationItem
            icon="location"
            title="New Services Available!"
            description="Now you can track orders in real time"
          />

          <Text style={[styles.sectionTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>December 22, 2026</Text>
          <NotificationItem
            icon="card"
            title="Credit Card Connected!"
            description="Credit Card has been linked!"
          />
          <NotificationItem
            icon="person"
            title="Account Setup Successful!"
            description="Your account has been created!"
          />
        </ScrollView>
      </View>
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
    padding: 16
  },
  headerContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: 'center'
  },
  headerIconContainer: {
    height: 46,
    width: 46,
    borderWidth: 1,
    borderColor: COLORS.grayscale200,
    alignItems: "center",
    justifyContent: "center",
    borderRadius: 999
  },
  arrowBackIcon: {
    width: 24,
    height: 24,
    tintColor: COLORS.black
  },
  headerTitle: {
    fontSize: 16,
    fontFamily: "bold",
    color: COLORS.black
  },
  headerNoti: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginVertical: 12
  },
  headerNotiLeft: {
    flexDirection: "row",
    alignItems: "center"
  },
  notiTitle: {
    fontSize: 16,
    fontFamily: "bold",
    color: COLORS.black
  },
  headerNotiView: {
    height: 16,
    width: 16,
    backgroundColor: COLORS.primary,
    borderRadius: 999,
    alignItems: 'center',
    justifyContent: 'center',
    marginLeft: 4
  },
  headerNotiTitle: {
    fontSize: 10,
    fontFamily: "bold",
    color: COLORS.white
  },
  clearAll: {
    fontSize: 14,
    color: COLORS.primary,
    fontFamily: "medium"
  },
  sectionTitle: {
    marginTop: 16,
    marginBottom: 8,
    fontWeight: 'bold',
    fontSize: 16,
  },
  notificationItem: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    padding: 12,
    marginBottom: 12,
  },
  iconContainer: {
    backgroundColor: '#000',
    borderRadius: 999,
    marginRight: 12,
    height: 68,
    width: 68,
    alignItems: "center",
    justifyContent: "center"
  },
  notificationTitle: {
    fontFamily: 'bold',
    fontSize: 18,
    color: COLORS.greyscale900,
    marginBottom: 6
  },
  notificationDescription: {
    color: 'gray',
    fontSize: 14,
    fontFamily: "medium"
  },
})

export default Notifications