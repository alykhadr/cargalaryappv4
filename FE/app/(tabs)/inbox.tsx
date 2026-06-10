import { View, StyleSheet, Image, TouchableOpacity, useWindowDimensions } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { TabView, SceneMap, TabBar } from 'react-native-tab-view';
import { Feather } from "@expo/vector-icons";
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { useTheme } from '@/theme/ThemeProvider';
import { COLORS, icons, SIZES } from '@/constants';
import { Calls, Chats } from '@/tabs';

const renderScene = SceneMap({
  first: Chats,
  second: Calls,
});

interface Route {
  key: string;
  title: string;
}

// Inbox tabs
const Inbox = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const layout = useWindowDimensions();
  const { colors, dark } = useTheme();

  const [index, setIndex] = React.useState(0);
  const [routes] = React.useState([
    { key: 'first', title: 'Chats' },
    { key: 'second', title: 'Calls' },
  ]);

  const renderTabBar = (props: any) => (
    <TabBar
      {...props}
      indicatorStyle={{
        backgroundColor: dark ? COLORS.white : COLORS.primary,
      }}
      style={{
        backgroundColor: colors.background,
      }}
      activeColor={dark ? COLORS.white : COLORS.primary}
      inactiveColor={dark ? COLORS.warning : COLORS.primary}
      renderLabel={({ route, focused }: { route: Route; focused: boolean }) => (
        <Text style={[{
          color: focused ? dark ? COLORS.white : COLORS.primary : 'gray',
          fontSize: 16,
          fontFamily: "bold"
        }]}>
          {route.title}
        </Text>
      )}
    />
  );
  /**
   * render header
   */
  const renderHeader = () => {
    return (
      <View style={styles.headerContainer}>
        <View style={styles.headerLeft}>
          <TouchableOpacity
            onPress={() => navigation.goBack()}>
            <Image
              source={icons.arrowLeft}
              resizeMode='contain'
              style={[styles.arrowLeftIcon, {
                tintColor: dark ? COLORS.white : COLORS.primary
              }]}
            />
          </TouchableOpacity>
          <Text style={[styles.headerTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Inbox</Text>
        </View>
        <View style={styles.headerRight}>
          <TouchableOpacity>
            <Image
              source={icons.search}
              resizeMode='contain'
              style={[styles.searchIcon, {
                tintColor: dark ? COLORS.secondaryWhite : COLORS.greyscale900
              }]}
            />
          </TouchableOpacity>
          <TouchableOpacity>
            <Image
              source={icons.moreCircle}
              resizeMode='contain'
              style={[styles.moreCircleIcon, {
                tintColor: dark ? COLORS.secondaryWhite : COLORS.greyscale900
              }]}
            />
          </TouchableOpacity>
        </View>
      </View>
    )
  }

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <TabView
          navigationState={{ index, routes }}
          renderScene={renderScene}
          onIndexChange={setIndex}
          initialLayout={{ width: layout.width }}
          renderTabBar={renderTabBar}
        />
        {/* Implementing adding post */}
        <TouchableOpacity style={[styles.addPostBtn, {
          backgroundColor: dark ? COLORS.white : COLORS.primary,
        }]}>
          <Feather name="plus" size={24} color={dark ? COLORS.primary : COLORS.white} />
        </TouchableOpacity>
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
    alignItems: "center",
    width: SIZES.width - 32,
    justifyContent: "space-between"
  },
  headerLeft: {
    flexDirection: "row",
    alignItems: "center"
  },
  arrowLeftIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.primary
  },
  headerTitle: {
    fontSize: 20,
    fontFamily: "bold",
    color: COLORS.black,
    marginLeft: 12
  },
  headerRight: {
    flexDirection: "row",
    alignItems: "center"
  },
  searchIcon: {
    width: 24,
    height: 24,
    tintColor: COLORS.black
  },
  moreCircleIcon: {
    width: 24,
    height: 24,
    tintColor: COLORS.black,
    marginLeft: 12
  },
  addPostBtn: {
    width: 58,
    height: 58,
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 999,
    backgroundColor: COLORS.primary,
    position: "absolute",
    bottom: 72,
    right: 16,
    zIndex: 999,
    shadowRadius: 10,
    shadowColor: COLORS.primary,
    shadowOpacity: 0.2,
    shadowOffset: { width: 0, height: 10 }
  }
})

export default Inbox