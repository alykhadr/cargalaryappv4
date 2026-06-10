import { View, StyleSheet, Alert } from 'react-native';
import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { COLORS, SIZES } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { useTheme } from '../theme/ThemeProvider';
import ButtonFilled from '../components/ButtonFilled';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { useLocalSearchParams } from 'expo-router';
import { api } from '@/services/api';
import { useAuth } from '@/context/AuthContext';

const MakeOffer = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { carId, carName } = useLocalSearchParams<{ carId: string; carName: string }>();
    const { colors, dark } = useTheme();
    const { user } = useAuth();

    const [name, setName] = useState(user?.fullName || '');
    const [phone, setPhone] = useState(user?.phoneNumber || '');
    const [message, setMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const handleSubmit = async () => {
        if (!name || !phone || !message) {
            Alert.alert('Error', 'Please fill in all fields');
            return;
        }
        setIsLoading(true);
        try {
            await api.submitInquiry(carId!, name, phone, message);
            navigation.navigate('makeofferprocessed');
        } catch (e: any) {
            Alert.alert('Error', e.message || 'Failed to submit inquiry');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <Header title="Contact Dealer" />
                <Text style={[styles.subtitle, { color: dark ? COLORS.greyscale300 : COLORS.greyScale800 }]}>
                    {carName ? `Inquiry about: ${carName}` : 'Send an inquiry to the dealer'}
                </Text>
                <TextInput
                    placeholder="Your Name"
                    placeholderTextColor={dark ? COLORS.grayTie : COLORS.gray}
                    style={[styles.input, {
                        color: dark ? COLORS.white : COLORS.greyscale900,
                        backgroundColor: dark ? COLORS.dark2 : COLORS.greyscale100,
                        borderColor: dark ? COLORS.dark3 : COLORS.greyscale200,
                    }]}
                    value={name}
                    onChangeText={setName}
                />
                <TextInput
                    placeholder="Phone Number"
                    placeholderTextColor={dark ? COLORS.grayTie : COLORS.gray}
                    style={[styles.input, {
                        color: dark ? COLORS.white : COLORS.greyscale900,
                        backgroundColor: dark ? COLORS.dark2 : COLORS.greyscale100,
                        borderColor: dark ? COLORS.dark3 : COLORS.greyscale200,
                    }]}
                    value={phone}
                    onChangeText={setPhone}
                    keyboardType="phone-pad"
                />
                <TextInput
                    placeholder="Message (e.g. I'm interested in this car...)"
                    placeholderTextColor={dark ? COLORS.grayTie : COLORS.gray}
                    style={[styles.messageInput, {
                        color: dark ? COLORS.white : COLORS.greyscale900,
                        backgroundColor: dark ? COLORS.dark2 : COLORS.greyscale100,
                        borderColor: dark ? COLORS.dark3 : COLORS.greyscale200,
                    }]}
                    value={message}
                    onChangeText={setMessage}
                    multiline
                    numberOfLines={5}
                    textAlignVertical="top"
                />
                <ButtonFilled
                    title={isLoading ? "Submitting..." : "Submit Inquiry"}
                    onPress={handleSubmit}
                    style={styles.button}
                />
            </View>
        </SafeAreaView>
    )
};

const styles = StyleSheet.create({
    area: { flex: 1, backgroundColor: COLORS.white },
    container: { flex: 1, backgroundColor: COLORS.white, padding: 16 },
    subtitle: {
        fontSize: 14,
        fontFamily: 'regular',
        color: COLORS.greyScale800,
        marginBottom: 24,
        marginTop: 8,
    },
    input: {
        width: SIZES.width - 32,
        height: 52,
        borderRadius: 12,
        borderWidth: 1,
        paddingHorizontal: 16,
        fontSize: 14,
        fontFamily: 'regular',
        marginBottom: 16,
    },
    messageInput: {
        width: SIZES.width - 32,
        height: 130,
        borderRadius: 12,
        borderWidth: 1,
        paddingHorizontal: 16,
        paddingTop: 12,
        fontSize: 14,
        fontFamily: 'regular',
        marginBottom: 24,
    },
    button: {
        width: SIZES.width - 32,
        borderRadius: 30,
    },
})

export default MakeOffer
