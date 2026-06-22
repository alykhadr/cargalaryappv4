import Checkbox from 'expo-checkbox';
import { LinearGradient } from 'expo-linear-gradient';
import { useNavigation } from 'expo-router';
import React, { useCallback, useEffect, useReducer, useState } from 'react';
import Text from '@/components/LocalizedText';
import { Alert, Image, ScrollView, StyleSheet, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import ButtonFilled from '../components/ButtonFilled';
import Input from '../components/Input';
import { COLORS, SIZES, icons } from '../constants';
import { useTheme } from '../theme/ThemeProvider';
import { validateInput } from '../utils/actions/formActions';
import { reducer } from '../utils/reducers/formReducers';
import { useAuth } from '../context/AuthContext';
import { useDirection } from '../theme/DirectionProvider';

const HERO_IMAGE = 'https://cdn.syarah.com/photos-thumbs/online-v1/0x426/online/posts/301247/orignal-301247-1-1778395884.jpg?v=3';

const initialState = {
    inputValues: { email: '', password: '' },
    inputValidities: { email: false, password: false },
    formIsValid: false,
};

type Nav = { navigate: (value: string) => void };

const Login = () => {
    const { navigate } = useNavigation<Nav>();
    const [formState, dispatchFormState] = useReducer(reducer, initialState);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isChecked, setChecked] = useState(false);
    const { colors, dark } = useTheme();
    const { language, setLanguage } = useDirection();
    const activeLang: 'EN' | 'AR' = language === 'Arabic' ? 'AR' : 'EN';
    const { login, continueAsGuest } = useAuth();

    const handleLangChange = async (lang: 'EN' | 'AR') => {
        await setLanguage(lang === 'EN' ? 'English' : 'Arabic');
    };

    const inputChangedHandler = useCallback(
        (inputId: string, inputValue: string) => {
            const result = validateInput(inputId, inputValue);
            dispatchFormState({ inputId, validationResult: result, inputValue });
        },
        [dispatchFormState]
    );

    useEffect(() => {
        if (error) {
            Alert.alert('Login Failed', error);
            setError(null);
        }
    }, [error]);

    const handleLogin = async () => {
        const email = formState.inputValues.email?.trim();
        const password = formState.inputValues.password;
        if (!email || !password) {
            Alert.alert('Error', 'Please enter email and password');
            return;
        }
        setIsLoading(true);
        try {
            const res = await login(email, password);
            navigate(res.needsProfile ? 'fillyourprofile' : '(tabs)');
        } catch (e: any) {
            setError(e.message || 'Login failed');
        } finally {
            setIsLoading(false);
        }
    };

    const handleGuest = () => {
        continueAsGuest();
        navigate('(tabs)');
    };

    return (
        <View style={styles.root}>
            {/* ── Hero Image ── */}
            <View style={styles.hero}>
                <Image source={{ uri: HERO_IMAGE }} style={StyleSheet.absoluteFillObject} resizeMode="cover" />
                <LinearGradient
                    colors={['rgba(0,0,0,0.08)', 'rgba(0,0,0,0.55)', 'rgba(0,0,0,0.90)']}
                    locations={[0, 0.55, 1]}
                    style={StyleSheet.absoluteFillObject}
                />


                {/* Branding — bottom left */}
                <View style={styles.heroBranding}>
                    <Text style={styles.heroAppName}>Carea</Text>
                    <Text style={styles.heroTagline}>{"Saudi's Premier Car Marketplace"}</Text>
                </View>
            </View>

            {/* ── Form card ── */}
            <SafeAreaView
                edges={['bottom']}
                style={[styles.formCard, { backgroundColor: colors.background }]}
            >
                <ScrollView
                    showsVerticalScrollIndicator={false}
                    contentContainerStyle={styles.scrollContent}
                    keyboardShouldPersistTaps="handled"
                >
                    <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.black }]}>
                        Login to Your Account
                    </Text>
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
                        id="password"
                        onInputChanged={inputChangedHandler}
                        errorText={formState.inputValidities['password']}
                        autoCapitalize="none"
                        placeholder="Password"
                        placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
                        icon={icons.padlock}
                        secureTextEntry={true}
                    />

                    <View style={styles.checkBoxContainer}>
                        <Checkbox
                            style={styles.checkbox}
                            value={isChecked}
                            color={isChecked ? COLORS.primary : dark ? COLORS.white : 'gray'}
                            onValueChange={setChecked}
                        />
                        <Text style={[styles.rememberText, { color: dark ? COLORS.white : COLORS.black }]}>
                            Remember me
                        </Text>
                    </View>

                    <ButtonFilled
                        title={isLoading ? 'Logging in…' : 'Login'}
                        onPress={handleLogin}
                        style={styles.button}
                    />

                    <TouchableOpacity onPress={() => navigate('forgotpasswordmethods')}>
                        <Text style={[styles.forgotText, { color: dark ? COLORS.white : COLORS.primary }]}>
                            Forgot the password?
                        </Text>
                    </TouchableOpacity>

                    <TouchableOpacity
                        style={[styles.guestBtn, { borderColor: dark ? COLORS.white : COLORS.primary }]}
                        onPress={handleGuest}
                    >
                        <Text style={[styles.guestBtnText, { color: dark ? COLORS.white : COLORS.primary }]}>
                            Continue as Guest
                        </Text>
                    </TouchableOpacity>

                    <View style={styles.signupRow}>
                        <Text style={[styles.signupLeft, { color: dark ? COLORS.white : COLORS.black }]}>
                            {"Don't have an account?"}
                        </Text>
                        <TouchableOpacity onPress={() => navigate('signup')}>
                            <Text style={[styles.signupRight, { color: dark ? '#E8001C' : COLORS.primary }]}>
                                {'  '}Sign Up
                            </Text>
                        </TouchableOpacity>
                    </View>
                </ScrollView>

                {/* ── Language strip — always visible at bottom ── */}
                <View style={[styles.langStrip, {
                    borderTopColor: dark ? 'rgba(255,255,255,0.10)' : 'rgba(0,0,0,0.08)',
                    backgroundColor: colors.background,
                }]}>
                    <Text style={[styles.langLabel, { color: dark ? 'rgba(255,255,255,0.85)' : COLORS.greyscale600 }]}>
                        Language
                    </Text>
                    <View style={[styles.langPill, {
                        backgroundColor: dark ? 'rgba(255,255,255,0.12)' : 'rgba(0,0,0,0.06)',
                    }]}>
                        <TouchableOpacity
                            onPress={() => handleLangChange('EN')}
                            style={[styles.langOption, activeLang === 'EN' && styles.langOptionActive]}
                            activeOpacity={0.8}
                        >
                            <Text style={[styles.langOptionText,
                                { color: dark ? 'rgba(255,255,255,0.80)' : COLORS.greyscale600 },
                                activeLang === 'EN' && styles.langOptionTextActive,
                            ]}>EN</Text>
                        </TouchableOpacity>
                        <TouchableOpacity
                            onPress={() => handleLangChange('AR')}
                            style={[styles.langOption, activeLang === 'AR' && styles.langOptionActive]}
                            activeOpacity={0.8}
                        >
                            <Text style={[styles.langOptionText,
                                { color: dark ? 'rgba(255,255,255,0.80)' : COLORS.greyscale600 },
                                activeLang === 'AR' && styles.langOptionTextActive,
                            ]}>AR</Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </SafeAreaView>
        </View>
    );
};

const styles = StyleSheet.create({
    root: {
        flex: 1,
        backgroundColor: '#000',
    },

    /* Hero */
    hero: {
        height: 260,
        overflow: 'hidden',
    },
    langStrip: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingHorizontal: 20,
        paddingVertical: 10,
        borderTopWidth: 1,
    },
    langLabel: {
        fontSize: 14,
        fontFamily: 'semiBold',
    },
    langPill: {
        flexDirection: 'row',
        borderRadius: 22,
        padding: 3,
    },
    langOption: {
        paddingHorizontal: 20,
        paddingVertical: 7,
        borderRadius: 18,
    },
    langOptionActive: {
        backgroundColor: '#E8001C',
    },
    langOptionText: {
        fontSize: 13,
        fontFamily: 'semiBold',
    },
    langOptionTextActive: {
        color: '#FFFFFF',
    },
    heroBranding: {
        position: 'absolute',
        bottom: 48,
        left: 20,
    },
    heroAppName: {
        fontSize: 36,
        fontFamily: 'extraBold',
        color: '#FFFFFF',
        letterSpacing: 0.5,
    },
    heroTagline: {
        fontSize: 13,
        fontFamily: 'regular',
        color: 'rgba(255,255,255,0.78)',
        marginTop: 2,
    },

    /* Form card */
    formCard: {
        flex: 1,
        borderTopLeftRadius: 28,
        borderTopRightRadius: 28,
        marginTop: -28,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: -4 },
        shadowOpacity: 0.12,
        shadowRadius: 12,
        elevation: 16,
    },
    scrollContent: {
        paddingHorizontal: 20,
        paddingTop: 28,
        paddingBottom: 24,
    },
    title: {
        fontSize: 24,
        fontFamily: 'bold',
        marginBottom: 20,
    },
    checkBoxContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        marginVertical: 14,
    },
    checkbox: {
        marginRight: 10,
        height: 16,
        width: 16,
        borderRadius: 4,
        borderColor: COLORS.primary,
        borderWidth: 2,
    },
    rememberText: {
        fontSize: 13,
        fontFamily: 'regular',
    },
    button: {
        marginVertical: 6,
        width: SIZES.width - 40,
        borderRadius: 30,
    },
    forgotText: {
        fontSize: 15,
        fontFamily: 'semiBold',
        textAlign: 'center',
        marginTop: 14,
        marginBottom: 4,
    },
    guestBtn: {
        marginTop: 14,
        width: SIZES.width - 40,
        height: 48,
        borderRadius: 30,
        borderWidth: 1.5,
        alignItems: 'center',
        justifyContent: 'center',
    },
    guestBtnText: {
        fontSize: 15,
        fontFamily: 'semiBold',
    },
    signupRow: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'center',
        marginTop: 20,
    },
    signupLeft: {
        fontSize: 14,
        fontFamily: 'regular',
    },
    signupRight: {
        fontSize: 15,
        fontFamily: 'semiBold',
    },
});

export default Login;
