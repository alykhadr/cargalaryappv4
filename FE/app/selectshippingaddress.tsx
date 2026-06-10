import { View, StyleSheet } from 'react-native';
import React, { useState } from 'react';
import { COLORS, SIZES } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTheme } from '../theme/ThemeProvider';
import Header from '../components/Header';
import { ScrollView } from 'react-native-virtualized-view';
import Button from '../components/Button';
import ButtonFilled from '../components/ButtonFilled';
import AddressItem from '@/components/AddressItem';
import { NavigationProp, useNavigation } from '@react-navigation/native';

const SelectShippingAddress = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { colors, dark } = useTheme();
    const [selectedItem, setSelectedItem] = useState(null);

    // Handle checkbox
    const handleCheckboxPress = (itemTitle: any) => {
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
                <Header title="Deliver To" />
                <ScrollView
                    contentContainerStyle={{
                        backgroundColor: dark ? COLORS.dark1 : COLORS.tertiaryWhite,
                        marginVertical: 12
                    }}
                    showsVerticalScrollIndicator={false}>
                    <AddressItem
                        checked={selectedItem === 'Home'}
                        onPress={() => handleCheckboxPress('Home')}
                        name="Home"
                        address="Times Square NYC, Manhattan 27"
                    />
                    <AddressItem
                        checked={selectedItem === 'My Office'}
                        onPress={() => handleCheckboxPress('My Office')}
                        name="My Office"
                        address="5259 Blue Bill Park, PC 4629"
                    />
                    <AddressItem
                        checked={selectedItem === 'My Appartment'}
                        onPress={() => handleCheckboxPress('My Appartment')}
                        name="My Appartment"
                        address="21833 Clyde Gallagher, PC 4629 "
                    />
                    <AddressItem
                        checked={selectedItem === "My Parent's House"}
                        onPress={() => handleCheckboxPress("My Parent's House")}
                        name="My Parent's House"
                        address="61480 Sunbrook Park, PC 45"
                    />
                    <AddressItem
                        checked={selectedItem === "My Villa"}
                        onPress={() => handleCheckboxPress("My Villa")}
                        name="My Villa"
                        address="61480 Sunbrook Park, PC 45"
                    />
                    <Button
                        title="Add New Address"
                        style={{
                            width: SIZES.width - 32,
                            borderRadius: 32,
                            backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
                            borderColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary
                        }}
                        textColor={dark ? COLORS.white : COLORS.primary}
                        onPress={() => navigation.navigate("addnewaddress")}
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

export default SelectShippingAddress