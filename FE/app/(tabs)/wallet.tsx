import { View, StyleSheet, TouchableOpacity, Image, FlatList } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { COLORS, icons, images, SIZES } from '@/constants';
import SectionHeader from '@/components/SectionHeader';
import { transactionHistory } from '@/data';
import TransactionHistoryItem from '@/components/TransactionHistoryItem';
import { useTheme } from '@/theme/ThemeProvider';
import { NavigationProp, useNavigation } from '@react-navigation/native';

const Wallet = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const { dark, colors } = useTheme();

  /**
   * Render header
   */
  const renderHeader = () => {
    return (
      <TouchableOpacity style={styles.headerContainer}>
        <View style={styles.headerLeft}>
          <Image
            source={images.logo}
            resizeMode='contain'
            style={[styles.logo, {
              tintColor: dark ? COLORS.white : COLORS.primary
            }]}
          />
          <Text style={[styles.headerTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>My Wallet</Text>
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
   * Render wallet card
   */
  const renderWalletCard = () => {
    return (
      <View style={styles.cardContainer}>
        <View style={styles.topCardContainer}>
          <View style={styles.topCardLeftContainer}>
            <Text style={styles.cardHolderName}>Andrew Ainsley</Text>
            <Text style={styles.cardNumber}>•••• •••• •••• ••••</Text>
          </View>
          <View style={styles.topCardRightContainer}>
            <Text style={styles.cardType}>VISA</Text>
            <Image
              source={icons.masterCardLogo}
              resizeMode='contain'
              style={styles.cardLogo}
            />
          </View>
        </View>
        <Text style={styles.balanceText}>Your balance</Text>
        <View style={styles.bottomCardContainer}>
          <Text style={styles.amountNumber}>$957,5</Text>
          <TouchableOpacity
            onPress={() => navigation.navigate("topupewalletamount")}
            style={styles.topupBtn}>
            <Image
              source={icons.arrowDownSquare}
              resizeMode='contain'
              style={styles.arrowDown}
            />
            <Text style={styles.topupBtnText}>Top Up</Text>
          </TouchableOpacity>
        </View>
      </View>
    )
  };

  const renderTransactionHistory = () => {
    return (
      <View>
        <SectionHeader
          title="Transaction History"
          subtitle="See All"
          onPress={() => navigation.navigate("transactionhistory")}
        />
        <FlatList
          data={transactionHistory.slice(0, 6)}
          keyExtractor={item => item.id}
          renderItem={({ item }) => (
            <TransactionHistoryItem
              image={item.image}
              name={item.name}
              date={item.date}
              type={item.type}
              amount={item.amount}
              onPress={() => console.log(item)}
            />
          )}
        />
      </View>
    )
  }
  
  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        <ScrollView showsVerticalScrollIndicator={false}>
          {renderWalletCard()}
          {renderTransactionHistory()}
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
    marginLeft: 12
  },
  headerIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.greyscale900
  },
  cardContainer: {
    width: SIZES.width - 32,
    borderRadius: 32,
    marginTop: 16,
    height: 212,
    backgroundColor: COLORS.primary,
    padding: 16
  },
  topCardContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    width: "100%",
  },
  topCardLeftContainer: {
    marginTop: 6
  },
  topCardRightContainer: {
    flexDirection: "row",
    alignItems: "center",
    marginBottom: 22
  },
  cardHolderName: {
    fontSize: 22,
    color: COLORS.white,
    fontFamily: "bold",
  },
  cardNumber: {
    fontSize: 20,
    color: COLORS.white,
    fontFamily: "semiBold",
  },
  cardType: {
    fontSize: 26,
    color: COLORS.white,
    fontFamily: "extraBoldItalic",
  },
  cardLogo: {
    height: 52,
    width: 52,
    marginLeft: 6
  },
  balanceText: {
    fontSize: 18,
    color: COLORS.white,
    fontFamily: "medium",
  },
  bottomCardContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginTop: 22
  },
  amountNumber: {
    fontSize: 42,
    color: COLORS.white,
    fontFamily: "bold",
  },
  topupBtn: {
    width: 132,
    height: 42,
    backgroundColor: COLORS.white,
    borderRadius: 18,
    alignItems: "center",
    justifyContent: "center",
    flexDirection: "row",
  },
  arrowDown: {
    width: 16,
    height: 16,
    tintColor: COLORS.black
  },
  topupBtnText: {
    fontSize: 16,
    color: COLORS.black,
    fontFamily: "semiBold",
    marginLeft: 12
  }
})

export default Wallet