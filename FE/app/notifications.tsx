import { NavigationProp } from '@react-navigation/native';
import { useNavigation } from 'expo-router';
import React, { useEffect, useMemo, useState } from 'react';
import Text from '@/components/LocalizedText';
import { Image, StyleSheet, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { COLORS, icons, images } from '../constants';
import { api } from '../services/api';
import { useTheme } from '../theme/ThemeProvider';
import { AppNotificationItem } from '../types';
import { getTimeAgo } from '../utils/date';
import { resolveRemoteImage } from '../utils/imageResolver';
import { useAuth } from '../context/AuthContext';

const Notifications = () => {
  const { colors, dark } = useTheme();
  const navigation = useNavigation<NavigationProp<any>>();
  const { user, isGuest } = useAuth();
  const [items, setItems] = useState<AppNotificationItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (isGuest || !user) {
      setLoading(false);
      setItems([]);
      return;
    }

    setLoading(true);
    api.getNotifications(25)
      .then((response) => setItems(response.items))
      .catch(() => setItems([]))
      .finally(() => setLoading(false));
  }, [isGuest, user]);

  const groupedNotifications = useMemo(() => {
    return items.reduce<Record<string, AppNotificationItem[]>>((acc, item) => {
      const date = new Date(item.createdDate);
      const key = date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      });

      if (!acc[key]) {
        acc[key] = [];
      }

      acc[key].push(item);
      return acc;
    }, {});
  }, [items]);

  const renderHeader = () => (
    <View style={styles.headerContainer}>
      <TouchableOpacity
        onPress={() => navigation.goBack()}
        style={[
          styles.headerIconContainer,
          { borderColor: dark ? COLORS.dark3 : COLORS.grayscale200 },
        ]}
      >
        <Image
          source={icons.back}
          resizeMode="contain"
          style={[styles.arrowBackIcon, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
        />
      </TouchableOpacity>
      <Text style={[styles.headerTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
        Notifications
      </Text>
      <Text>{'  '}</Text>
    </View>
  );

  const renderBody = () => {
    if (loading) {
      return (
        <Text style={[styles.stateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
          Loading notifications...
        </Text>
      );
    }

    if (isGuest || !user) {
      return (
        <Text style={[styles.stateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
          Login to view your latest notifications.
        </Text>
      );
    }

    if (items.length === 0) {
      return (
        <Text style={[styles.stateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
          No notifications yet.
        </Text>
      );
    }

    return Object.entries(groupedNotifications).map(([sectionTitle, sectionItems]) => (
      <View key={sectionTitle} style={styles.sectionWrap}>
        <Text style={[styles.sectionTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
          {sectionTitle}
        </Text>
        {sectionItems.map((item) => (
          <View
            key={item.id}
            style={[
              styles.notificationItem,
              { backgroundColor: dark ? COLORS.dark2 : COLORS.grayscale100 },
            ]}
          >
            <Image
              source={resolveRemoteImage(item.carImageUrl, images.honda1)}
              resizeMode="cover"
              style={styles.notificationImage}
            />
            <View style={styles.notificationContent}>
              <Text style={[styles.notificationTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                New inquiry received
              </Text>
              <Text style={[styles.notificationDescription, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
                {item.carName}
              </Text>
              <Text style={[styles.notificationDate, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
                {getTimeAgo(item.createdDate)}
              </Text>
            </View>
          </View>
        ))}
      </View>
    ));
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <ScrollView showsVerticalScrollIndicator={false}>
          {renderBody()}
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
  headerContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  headerIconContainer: {
    height: 46,
    width: 46,
    borderWidth: 1,
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 999,
  },
  arrowBackIcon: {
    width: 24,
    height: 24,
  },
  headerTitle: {
    fontSize: 16,
    fontFamily: 'bold',
  },
  sectionWrap: {
    marginTop: 18,
  },
  sectionTitle: {
    marginBottom: 10,
    fontSize: 16,
    fontFamily: 'bold',
  },
  notificationItem: {
    flexDirection: 'row',
    borderRadius: 18,
    padding: 12,
    marginBottom: 12,
  },
  notificationImage: {
    width: 68,
    height: 68,
    borderRadius: 14,
    marginRight: 12,
    backgroundColor: COLORS.grayscale200,
  },
  notificationContent: {
    flex: 1,
    justifyContent: 'center',
  },
  notificationTitle: {
    fontSize: 16,
    fontFamily: 'bold',
    marginBottom: 4,
  },
  notificationDescription: {
    fontSize: 14,
    fontFamily: 'medium',
    marginBottom: 6,
  },
  notificationDate: {
    fontSize: 12,
    fontFamily: 'regular',
  },
  stateText: {
    fontSize: 15,
    fontFamily: 'medium',
    textAlign: 'center',
    marginTop: 48,
    lineHeight: 22,
  },
});

export default Notifications;
