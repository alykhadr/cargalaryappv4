import Input from '@/components/Input';
import Checkbox from 'expo-checkbox';
import { useNavigation } from 'expo-router';
import React, { useCallback, useEffect, useReducer, useState } from 'react';
import Text from '@/components/LocalizedText';
import { Alert, Image, ScrollView, StyleSheet, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import ButtonFilled from '../components/ButtonFilled';
import Header from '../components/Header';
import { COLORS, SIZES, icons, images } from '../constants';
import { useTheme } from '../theme/ThemeProvider';
import { validateInput } from '../utils/actions/formActions';
import { reducer } from '../utils/reducers/formReducers';
import { useAuth } from '../context/AuthContext';

const initialState = {
    inputValues: {
        email: '',
        password: '',
    },
    inputValidities: {
        email: false,
        password: false
    },
    formIsValid: false,
}

type Nav = {
    navigate: (value: string, params?: object) => void
}

const Signup = () => {
    const { navigate } = useNavigation<Nav>();
    const [formState, dispatchFormState] = useReducer(reducer, initialState);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isChecked, setChecked] = useState(false);
    const { colors, dark } = useTheme();
    const { signup } = useAuth();

    const inputChangedHandler = useCallback(
        (inputId: string, inputValue: string) => {
            const result = validateInput(inputId, inputValue)
            dispatchFormState({
                inputId,
                validationResult: result,
                inputValue,
            })
        },
        [dispatchFormState]);

    useEffect(() => {
        if (error) {
            Alert.alert('Signup Failed', error);
            setError(null);
        }
    }, [error]);

    const handleSignup = async () => {
        const email = formState.inputValues.email?.trim();
        const password = formState.inputValues.password;
        if (!email || !password) {
            Alert.alert('Error', 'Please enter email and password');
            return;
        }
        setIsLoading(true);
        try {
            const { userId } = await signup(email, password);
            navigate('fillyourprofile', { userId });
        } catch (e: any) {
            setError(e.message || 'Signup failed');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <Header title="" />
                <ScrollView showsVerticalScrollIndicator={false}>
                    <View style={styles.logoContainer}>
                        <Image
                            source={images.logo}
                            resizeMode='contain'
                            style={[styles.logo, {
                                tintColor: dark ? COLORS.white : COLORS.black
                            }]}
                        />
                    </View>
                    <Text style={[styles.title, {
                        color: dark ? COLORS.white : COLORS.black
                    }]}>Create Your Account</Text>
                    <Input
                        id="email"
                        onInputChanged={inputChangedHandler}
                        errorText={formState.inputValidities['email']}
                        placeholder="Email"
                        placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
                        icon={icons.email}
                        keyboardType="email-address"
                    />
                    <Input
                        onInputChanged={inputChangedHandler}
                        errorText={formState.inputValidities['password']}
                        autoCapitalize="none"
                        id="password"
                        placeholder="Password"
                        placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
                        icon={icons.padlock}
                        secureTextEntry={true}
                    />
                    <View style={styles.checkBoxContainer}>
                        <View style={{ flexDirection: 'row' }}>
                            <Checkbox
                                style={styles.checkbox}
                                value={isChecked}
                                color={isChecked ? COLORS.primary : dark ? COLORS.white : "gray"}
                                onValueChange={setChecked}
                            />
                            <View style={{ flex: 1 }}>
                                <Text style={[styles.privacy, {
                                    color: dark ? COLORS.white : COLORS.black
                                }]}>By continuing you accept our Privacy Policy</Text>
                            </View>
                        </View>
                    </View>
                    <ButtonFilled
                        title={isLoading ? "Creating account..." : "Sign Up"}
                        onPress={handleSignup}
                        style={styles.button}
                    />
                </ScrollView>
                <View style={styles.bottomContainer}>
                    <Text style={[styles.bottomLeft, {
                        color: dark ? COLORS.white : COLORS.black
                    }]}>Already have an account ?</Text>
                    <TouchableOpacity
                        onPress={() => navigate("login")}>
                        <Text style={[styles.bottomRight, {
                            color: dark ? COLORS.white : COLORS.primary
                        }]}>{" "}Sign In</Text>
                    </TouchableOpacity>
                </View>
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
        padding: 16,
        backgroundColor: COLORS.white
    },
    logo: {
        width: 100,
        height: 100,
        tintColor: COLORS.primary
    },
    logoContainer: {
        alignItems: "center",
        justifyContent: "center",
        marginVertical: 32
    },
    title: {
        fontSize: 26,
        fontFamily: "bold",
        color: COLORS.black,
        textAlign: "center",
        marginBottom: 22
    },
    checkBoxContainer: {
        flexDirection: "row",
        justifyContent: 'space-between',
        alignItems: 'center',
        marginVertical: 18,
    },
    checkbox: {
        marginRight: 8,
        height: 16,
        width: 16,
        borderRadius: 4,
        borderColor: COLORS.primary,
        borderWidth: 2,
    },
    privacy: {
        fontSize: 12,
        fontFamily: "regular",
        color: COLORS.black,
    },
    bottomContainer: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "center",
        marginVertical: 18,
        position: "absolute",
        bottom: 12,
        right: 0,
        left: 0,
    },
    bottomLeft: {
        fontSize: 14,
        fontFamily: "regular",
        color: "black"
    },
    bottomRight: {
        fontSize: 16,
        fontFamily: "medium",
        color: COLORS.primary
    },
    button: {
        marginVertical: 6,
        width: SIZES.width - 32,
        borderRadius: 30
    }
})

export default Signup
