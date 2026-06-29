import { View, StyleSheet, ScrollView, Image, TouchableOpacity } from 'react-native';
import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, SIZES, icons, illustrations } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { useTheme } from '../theme/ThemeProvider';
import ButtonFilled from '../components/ButtonFilled';
import { useNavigation } from 'expo-router';

type Nav = {
    navigate: (value: string) => void
};

const ForgotPasswordMethods = () => {
    const { navigate } = useNavigation<Nav>();
    const [selectedMethod, setSelectedMethod] = useState('username');
    const { colors, dark } = useTheme();

    const handleMethodPress = (method: any) => {
        setSelectedMethod(method);
    };
    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <Header title="Forgot Password" />
                <ScrollView showsVerticalScrollIndicator={false}>
                    <View style={styles.passwordContainer}>
                        <Image
                            source={dark ? illustrations.passwordDark : illustrations.password}
                            resizeMode='contain'
                            style={styles.password}
                        />
                    </View>
                    <Text style={[styles.title, {
                        color: dark ? COLORS.white : COLORS.greyscale900
                    }]}>Choose how you want to find your account before we send a reset link</Text>
                    <TouchableOpacity
                        style={[
                            styles.methodContainer,
                            selectedMethod === 'username' && { borderColor: dark ? COLORS.white : COLORS.primary, borderWidth: 2 },
                        ]}
                        onPress={() => handleMethodPress('username')}>
                        <View style={styles.iconContainer}>
                            <Image
                                source={icons.chat}
                                resizeMode='contain'
                                style={[styles.icon,
                                selectedMethod === 'username' && { tintColor: dark ? COLORS.white : COLORS.primary },
                                ]} />
                        </View>
                        <View>
                            <Text style={styles.methodTitle}>via Username:</Text>
                            <Text style={[styles.methodSubtitle, {
                                color: dark ? COLORS.white : COLORS.black
                            }]}>Use the same username you log in with</Text>
                        </View>
                    </TouchableOpacity>
                    <TouchableOpacity
                        style={[
                            styles.methodContainer,
                            selectedMethod === 'email' && { borderColor: dark ? COLORS.white : COLORS.primary, borderWidth: 2 }, // Customize the border color for Email
                        ]}
                        onPress={() => handleMethodPress('email')}>
                        <View style={styles.iconContainer}>
                            <Image
                                source={icons.email}
                                resizeMode='contain'
                                style={[styles.icon, selectedMethod === 'email' && { tintColor: dark ? COLORS.white : COLORS.primary },]} />
                        </View>
                        <View>
                            <Text style={styles.methodTitle}>via Email:</Text>
                            <Text style={[styles.methodSubtitle, {
                                color: dark ? COLORS.white : COLORS.black
                            }]}>Send the reset link to your email address</Text>
                        </View>
                    </TouchableOpacity>
                    <ButtonFilled
                        title="Continue"
                        style={styles.button}
                        onPress={() =>
                            navigate(
                                selectedMethod === "username"
                                    ? 'forgotpasswordphonenumber'
                                    : 'forgotpasswordemail'
                            )
                        }
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
    password: {
        width: 276,
        height: 250
    },
    passwordContainer: {
        alignItems: "center",
        justifyContent: "center",
        marginVertical: 32
    },
    title: {
        fontSize: 18,
        fontFamily: "medium",
        color: COLORS.greyscale900
    },
    methodContainer: {
        width: SIZES.width - 32,
        height: 112,
        borderRadius: 32,
        borderColor: "gray",
        borderWidth: .3,
        flexDirection: "row",
        alignItems: "center",
        marginTop: 22
    },
    iconContainer: {
        width: 80,
        height: 80,
        borderRadius: 40,
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: COLORS.tansparentPrimary,
        marginHorizontal: 16
    },
    icon: {
        width: 32,
        height: 32,
        tintColor: COLORS.primary
    },
    methodTitle: {
        fontSize: 14,
        fontFamily: "medium",
        color: COLORS.greyscale600
    },
    methodSubtitle: {
        fontSize: 16,
        fontFamily: "bold",
        color: COLORS.black,
        marginTop: 12
    },
    button: {
        borderRadius: 32,
        marginVertical: 22
    }
})

export default ForgotPasswordMethods
