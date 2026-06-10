import { View, StyleSheet, TouchableOpacity, Image, FlatList } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import { COLORS, SIZES, icons } from '../constants';
import HeaderWithSearch from '../components/HeaderWithSearch';
import { orderList } from '../data';
import { Feather } from "@expo/vector-icons";
import ButtonFilled from '../components/ButtonFilled';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import OrderListItem from '@/components/OrderListItem';

const Checkout = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const { colors, dark } = useTheme();

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <HeaderWithSearch
          title="Checkout"
          icon={icons.moreCircle}
          onPress={() => null}
        />
        <ScrollView
          contentContainerStyle={{
            backgroundColor: dark ? COLORS.dark1 : COLORS.tertiaryWhite,
            marginTop: 12
          }}
          showsVerticalScrollIndicator={false}>
          <Text style={[styles.summaryTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Shipping Address</Text>
          <View style={[styles.summaryContainer, {
            backgroundColor: dark ? COLORS.dark2 : COLORS.white,
          }]}>
            <View style={[styles.separateLine, {
              backgroundColor: dark ? COLORS.grayscale700 : COLORS.grayscale200
            }]} />
            <TouchableOpacity
              onPress={() => navigation.navigate("selectshippingaddress")}
              style={styles.addressContainer}>
              <View style={styles.addressLeftContainer}>
                <View style={styles.view1}>
                  <View style={styles.view2}>
                    <Image
                      source={icons.location2}
                      resizeMode='contain'
                      style={styles.locationIcon}
                    />
                  </View>
                </View>
                <View style={styles.viewAddress}>
                  <View style={styles.viewView}>
                    <Text style={[styles.homeTitle, {
                      color: dark ? COLORS.white : COLORS.greyscale900
                    }]}>Home</Text>
                    <View style={styles.defaultView}>
                      <Text style={styles.defaultTitle}>Default</Text>
                    </View>
                  </View>
                  <Text style={[styles.addressTitle, {
                    color: dark ? COLORS.grayscale200 : COLORS.grayscale700
                  }]}>
                    Time Square NYC, Nanhattan</Text>
                </View>
              </View>
              <Image
                source={icons.arrowRight}
                resizeMode='contain'
                style={[styles.arrowRightIcon, {
                  tintColor: dark ? COLORS.white : COLORS.greyscale900
                }]}
              />
            </TouchableOpacity>
          </View>

          <Text style={[styles.summaryTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Order List</Text>
          <FlatList
            data={orderList}
            keyExtractor={item => item.id}
            style={{ marginTop: 12 }}
            renderItem={({ item }) => (
              <OrderListItem
                name={item.name}
                image={item.image}
                price={item.price}
                rating={item.rating}
                numReviews={item.numReviews}
                color={item.color}
                quantity={item.quantity}
              />
            )}
          />
          <View style={[styles.separateLine, {
            backgroundColor: dark ? COLORS.grayscale700 : COLORS.grayscale200
          }]} />
          <Text style={[styles.summaryTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Choose Shipping</Text>

          <TouchableOpacity
            onPress={() => navigation.navigate("chooseshippingmethods")}
            style={[styles.addressContainer, styles.shippingMethods, { 
              backgroundColor: dark ? COLORS.dark3 : COLORS.white,
              borderRadius: 12
            }]}>
            <View style={styles.addressLeftContainer}>
              <Image
                source={icons.cargoTruck}
                resizeMode='contain'
                style={[styles.locationIcon, {
                  tintColor: dark ? COLORS.white : COLORS.greyscale900,
                  marginLeft: 8
                }]}
              />
              <View style={styles.viewAddress}>
                <View style={styles.viewView}>
                  <Text style={[styles.homeTitle, {
                    color: dark ? COLORS.white : COLORS.greyscale900
                  }]}>Choose Shipping Type</Text>
                </View>
              </View>
            </View>
            <Image
              source={icons.arrowRight}
              resizeMode='contain'
              style={[styles.arrowRightIcon, {
                tintColor: dark ? COLORS.white : COLORS.greyscale900
              }]}
            />
          </TouchableOpacity>
          <View style={[styles.separateLine, {
            backgroundColor: dark ? COLORS.grayscale700 : COLORS.grayscale200,
            marginTop: 4,
            marginBottom: 16
          }]} />

          <Text style={[styles.summaryTitle, {
            color: dark ? COLORS.white : COLORS.greyscale900
          }]}>Promo Code</Text>
          <View style={[styles.promoCodeContainer, { 
            backgroundColor: "transparent",
          }]}>
            <TextInput
              placeholder='Enter Promo Code'
              placeholderTextColor={dark ? COLORS.white : COLORS.grayscale700}
              style={[styles.codeInput, {
                color: dark ? COLORS.secondaryWhite : COLORS.greyscale900,
                backgroundColor: dark ? COLORS.dark2 : COLORS.white,
              }]}
            />
            <TouchableOpacity
              onPress={() => navigation.navigate("addpromo")}
              style={[styles.addPromoBtn, { 
                backgroundColor: dark ? COLORS.dark3 : COLORS.primary,
              }]}>
              <Feather name="plus" size={18} color={COLORS.white} />
            </TouchableOpacity>
          </View>

          <View style={[styles.summaryContainer, {
            backgroundColor: dark ? COLORS.dark2 : COLORS.white,
          }]}>
            <View style={styles.view}>
              <Text style={[styles.viewLeft, {
                color: dark ? COLORS.grayscale200 : COLORS.grayscale700
              }]}>Subtitle</Text>
              <Text style={[styles.viewRight, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>$1920.00</Text>
            </View>
            <View style={styles.view}>
              <Text style={[styles.viewLeft, {
                color: dark ? COLORS.grayscale200 : COLORS.grayscale700
              }]}>Delivery Fee</Text>
              <Text style={[styles.viewRight, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>$2.00</Text>
            </View>
            <View style={styles.view}>
              <Text style={[styles.viewLeft, {
                color: dark ? COLORS.grayscale200 : COLORS.grayscale700
              }]}>Promo</Text>
              <Text style={[styles.viewRight, { color: dark ? COLORS.primary : COLORS.primary }]}>- $12.80</Text>
            </View>
            <View style={[styles.separateLine, {
              backgroundColor: dark ? COLORS.greyScale800 : COLORS.grayscale200
            }]} />
            <View style={styles.view}>
              <Text style={[styles.viewLeft, {
                color: dark ? COLORS.grayscale200 : COLORS.grayscale700
              }]}>Total</Text>
              <Text style={[styles.viewRight, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>$1909.20</Text>
            </View>
          </View>
        </ScrollView>
      </View>
      <View style={[styles.buttonContainer, { 
        backgroundColor: dark ? COLORS.dark2 : COLORS.white,
      }]}>
        <ButtonFilled
          title="Continue to Payment"
          onPress={() => navigation.navigate("paymentmethods")}
          style={styles.placeOrderButton}
        />
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
    paddingHorizontal: 16,
    paddingTop: 16
  },
  summaryContainer: {
    width: SIZES.width - 32,
    borderRadius: 16,
    padding: 16,
    backgroundColor: COLORS.white,
    shadowColor: COLORS.black,
    shadowOffset: {
      width: 0,
      height: 0
    },
    shadowOpacity: 0.2,
    shadowRadius: 0,
    elevation: 0,
    marginBottom: 12,
    marginTop: 12,
  },
  view: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginVertical: 12
  },
  viewLeft: {
    fontSize: 14,
    fontFamily: "medium",
    color: COLORS.grayscale700
  },
  viewRight: {
    fontSize: 14,
    fontFamily: "semiBold",
    color: COLORS.greyscale900
  },
  separateLine: {
    width: "100%",
    height: 1,
    backgroundColor: COLORS.grayscale200,
    marginVertical: 12
  },
  summaryTitle: {
    fontSize: 20,
    fontFamily: "bold",
    color: COLORS.greyscale900
  },
  addressContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  addressLeftContainer: {
    flexDirection: "row",
    alignItems: "center",
  },
  view1: {
    height: 52,
    width: 52,
    borderRadius: 26,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.tansparentPrimary,
  },
  view2: {
    height: 38,
    width: 38,
    borderRadius: 19,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.primary,
  },
  locationIcon: {
    height: 20,
    width: 20,
    tintColor: COLORS.white
  },
  viewView: {
    flexDirection: "row",
    alignItems: "center",
  },
  homeTitle: {
    fontSize: 18,
    fontFamily: "bold",
    color: COLORS.greyscale900
  },
  defaultView: {
    width: 64,
    height: 26,
    borderRadius: 6,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.tansparentPrimary,
    marginHorizontal: 12
  },
  defaultTitle: {
    fontSize: 12,
    fontFamily: "medium",
    color: COLORS.primary,
  },
  addressTitle: {
    fontSize: 14,
    fontFamily: "medium",
    color: COLORS.grayscale700,
    marginVertical: 4
  },
  viewAddress: {
    marginHorizontal: 16
  },
  arrowRightIcon: {
    height: 16,
    width: 16,
    tintColor: COLORS.greyscale900
  },
  orderSummaryView: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between"
  },
  addItemView: {
    width: 78,
    height: 26,
    borderRadius: 32,
    justifyContent: "center",
    alignItems: "center",
    borderColor: COLORS.primary,
    borderWidth: 1.4,
  },
  addItemText: {
    fontSize: 12,
    fontFamily: "medium",
    color: COLORS.primary,
  },
  viewItemTypeContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center"
  },
  viewLeftItemTypeContainer: {
    flexDirection: "row",
    alignItems: "center",
  },
  walletIcon: {
    height: 24,
    width: 24,
    tintColor: COLORS.primary,
    marginRight: 16
  },
  viewItemTypeTitle: {
    fontSize: 14,
    fontFamily: "medium",
    color: COLORS.grayscale700,
    marginRight: 16
  },
  placeOrderButton: {
    marginBottom: 12,
    marginTop: 6
  },
  shippingMethods: {
    backgroundColor: COLORS.white,
    paddingVertical: 20,
    marginVertical: 16
  },
  promoCodeContainer: {
    width: SIZES.width - 32,
    flexDirection: "row",
    justifyContent: "space-between",
    backgroundColor: COLORS.white,
    marginVertical: 12
  },
  codeInput: {
    width: SIZES.width - 112,
    height: 56,
    borderRadius: 16,
    backgroundColor: "#FAFAFA",
    paddingHorizontal: 16,
    paddingVertical: 12,
    fontSize: 14,
  },
  addPromoBtn: {
    height: 48,
    width: 48,
    borderRadius: 24,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: COLORS.primary,
  },
  buttonContainer: {
    position: "absolute",
    bottom: 12,
    width: SIZES.width,
    paddingHorizontal: 16,
    paddingTop: 12,
    backgroundColor: COLORS.white,
    borderTopLeftRadius: 32,
    borderTopRightRadius: 32,
  }
});

export default Checkout