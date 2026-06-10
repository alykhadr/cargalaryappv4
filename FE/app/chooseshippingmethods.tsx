import { View, StyleSheet } from 'react-native';
import React, { useState } from 'react';
import { COLORS, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTheme } from '../theme/ThemeProvider';
import Header from '../components/Header';
import { ScrollView } from 'react-native-virtualized-view';
import ButtonFilled from '../components/ButtonFilled';
import ShippingItem from '@/components/ShippingItem';
import { NavigationProp, useNavigation } from '@react-navigation/native';

const ChooseShippingMethods = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const { colors, dark } = useTheme();
  const [selectedItem, setSelectedItem] = useState(null);

  // Handle checkbox
  const handleCheckboxPress = (itemTitle:any) => {
    if (selectedItem === itemTitle) {
      // If the clicked item is already selected, deselect it
      setSelectedItem(null);
    } else {
      // Otherwise, select the clicked item
      setSelectedItem(itemTitle);
    }
  };

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <Header title="Choose Shipping" />
        <ScrollView
          contentContainerStyle={{
            backgroundColor: dark ? COLORS.dark1 : COLORS.tertiaryWhite,
            marginVertical: 12
          }}
          showsVerticalScrollIndicator={false}>
          <ShippingItem
            checked={selectedItem === 'Economy'}
            onPress={() => handleCheckboxPress('Economy')}
            name="Economy"
            arrivalDate="Dec 20-23"
            price={100}
            icon={icons.box2}
          />
          <ShippingItem
            checked={selectedItem === 'Regular'}
            onPress={() => handleCheckboxPress('Regular')}
            name="Regular"
            arrivalDate="Dec 20-22"
            price={150}
            icon={icons.box3}
          />
          <ShippingItem
            checked={selectedItem === 'Cargo'}
            onPress={() => handleCheckboxPress('Cargo')}
            name="Cargo"
            arrivalDate="Dec 19-20"
            price={200}
            icon={icons.cargo}
          />
          <ShippingItem
            checked={selectedItem === "Express"}
            onPress={() => handleCheckboxPress("Express")}
            name="Express"
            arrivalDate="Dec 18-19"
            price={300}
            icon={icons.cargo2}
          />
          <ShippingItem
            checked={selectedItem === "Premium"}
            onPress={() => handleCheckboxPress("Premium")}
            name="Premium"
            arrivalDate="Dec 17-18"
            price={500}
            icon={icons.cargo2}
          />
        </ScrollView>
        <ButtonFilled
          title="Apply"
          onPress={() => navigation.goBack()}
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
    paddingTop: 16,
    paddingHorizontal: 16
  },
  addBtn: {
    backgroundColor: COLORS.tansparentPrimary,
    borderColor: COLORS.tansparentPrimary
  }
})

export default ChooseShippingMethods