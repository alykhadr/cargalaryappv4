import { useRouter } from "expo-router";
import React, { useEffect } from "react";
import Text from '@/components/LocalizedText';
import { Image, Platform, Pressable, StyleSheet, TouchableOpacity, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { LinearGradient } from "expo-linear-gradient";
import { COLORS, illustrations, images } from "../constants";
import { useTheme } from "../theme/ThemeProvider";
import { useAuth } from "../context/AuthContext";
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withTiming,
    withDelay,
    Easing,
} from 'react-native-reanimated';

const Welcome = () => {
    const router = useRouter();
    const { dark } = useTheme();
    const { user, isGuest, isLoading } = useAuth();

    const brandOpacity = useSharedValue(0);
    const brandY = useSharedValue(28);
    const titleOpacity = useSharedValue(0);
    const titleY = useSharedValue(20);
    const btnOpacity = useSharedValue(0);
    const btnY = useSharedValue(20);

    useEffect(() => {
        const cfg = { duration: 580, easing: Easing.out(Easing.cubic) };
        brandOpacity.value = withDelay(180, withTiming(1, cfg));
        brandY.value = withDelay(180, withTiming(0, cfg));
        titleOpacity.value = withDelay(360, withTiming(1, cfg));
        titleY.value = withDelay(360, withTiming(0, cfg));
        btnOpacity.value = withDelay(560, withTiming(1, cfg));
        btnY.value = withDelay(560, withTiming(0, cfg));
    }, []);

    useEffect(() => {
        if (!isLoading && (user || isGuest)) {
            router.replace('/(tabs)');
        }
    }, [isGuest, isLoading, router, user]);

    const brandStyle = useAnimatedStyle(() => ({
        opacity: brandOpacity.value,
        transform: [{ translateY: brandY.value }],
    }));

    const titleStyle = useAnimatedStyle(() => ({
        opacity: titleOpacity.value,
        transform: [{ translateY: titleY.value }],
    }));

    const btnStyle = useAnimatedStyle(() => ({
        opacity: btnOpacity.value,
        transform: [{ translateY: btnY.value }],
    }));

    return (
        <View style={styles.area}>
            <Image
                source={dark ? illustrations.welcomeDark : images.tesla5}
                resizeMode="cover"
                style={StyleSheet.absoluteFillObject}
            />
            <LinearGradient
                colors={
                    dark
                        ? ['transparent', 'transparent', 'rgba(0,0,0,0.65)', 'rgba(0,0,0,0.92)', '#000000']
                        : ['transparent', 'transparent', 'rgba(255,255,255,0.70)', 'rgba(255,255,255,0.95)', '#FFFFFF']
                }
                locations={[0, 0.28, 0.52, 0.72, 1]}
                style={StyleSheet.absoluteFillObject}
            />
            <SafeAreaView style={styles.safeArea}>
                <View style={styles.bottomContainer}>
                    <Animated.View style={[styles.brandRow, brandStyle]}>
                        <Text style={[styles.brandName, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                            Carea
                        </Text>
                        <Text style={[styles.brandTagline, { color: dark ? 'rgba(255,255,255,0.65)' : COLORS.grayscale700 }]}>
                            Drive your dream
                        </Text>
                    </Animated.View>
                    <Animated.View style={titleStyle}>
                        <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                            Find Your{'\n'}Perfect Car
                        </Text>
                    </Animated.View>
                    <Animated.View style={btnStyle}>
                        <Pressable
                            onPress={() => router.push('/login')}
                            style={({ pressed }) => [
                                styles.getStartedButton,
                                { backgroundColor: dark ? COLORS.white : COLORS.primary, opacity: pressed ? 0.86 : 1 },
                            ]}
                        >
                            <Text style={[styles.getStartedText, { color: dark ? '#101010' : COLORS.white }]}>
                                Get Started
                            </Text>
                        </Pressable>
                        <View style={styles.signupRow}>
                            <Text style={[styles.loginTitle, { color: dark ? 'rgba(255,255,255,0.7)' : COLORS.grayscale700 }]}>
                                {"Don't have an account?"}
                            </Text>
                            <TouchableOpacity onPress={() => router.push('/signup')} style={styles.signupLink}>
                                <Text style={[styles.loginSubtitle, { color: dark ? COLORS.white : COLORS.primary }]}>
                                    Sign up
                                </Text>
                            </TouchableOpacity>
                        </View>
                    </Animated.View>
                </View>
            </SafeAreaView>
        </View>
    );
};

const styles = StyleSheet.create({
    area: {
        flex: 1,
        backgroundColor: '#000',
    },
    safeArea: {
        flex: 1,
        justifyContent: 'flex-end',
    },
    bottomContainer: {
        paddingHorizontal: 24,
        paddingBottom: Platform.OS === 'ios' ? 24 : 32,
    },
    brandRow: {
        marginBottom: 6,
    },
    brandName: {
        fontSize: 38,
        fontFamily: 'black',
        letterSpacing: -1,
        lineHeight: 42,
    },
    brandTagline: {
        fontSize: 14,
        fontFamily: 'regular',
        marginTop: 2,
    },
    title: {
        fontSize: 26,
        fontFamily: 'bold',
        lineHeight: 34,
        marginTop: 4,
    },
    getStartedButton: {
        width: '100%',
        height: 64,
        marginTop: 20,
        marginBottom: 16,
        borderRadius: 32,
        alignItems: 'center',
        justifyContent: 'center',
    },
    getStartedText: {
        fontSize: 18,
        fontFamily: 'semiBold',
        textAlign: 'center',
    },
    signupRow: {
        alignItems: 'center',
        justifyContent: 'center',
        rowGap: 4,
    },
    signupLink: {
        minHeight: 28,
        justifyContent: 'center',
    },
    loginTitle: {
        fontSize: 14,
        fontFamily: 'regular',
        textAlign: 'center',
        flexShrink: 1,
    },
    loginSubtitle: {
        fontSize: 14,
        fontFamily: 'bold',
        textAlign: 'center',
    },
});

export default Welcome;
