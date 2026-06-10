import Button from "@/components/Button";
import { useNavigation } from "expo-router";
import React, { useEffect } from "react";
import Text from '@/components/LocalizedText';
import { Image, Platform, StyleSheet, TouchableOpacity, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { LinearGradient } from "expo-linear-gradient";
import { COLORS, illustrations, images } from "../constants";
import { useTheme } from "../theme/ThemeProvider";
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withTiming,
    withDelay,
    Easing,
} from 'react-native-reanimated';

type Nav = {
    navigate: (value: string) => void
}

const Welcome = () => {
    const { navigate } = useNavigation<Nav>();
    const { dark } = useTheme();

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
                        <Button
                            title="Get Started"
                            onPress={() => navigate('login')}
                            textColor={dark ? '#101010' : COLORS.white}
                            style={{
                                width: '100%',
                                marginTop: 20,
                                marginBottom: 16,
                                backgroundColor: dark ? COLORS.white : COLORS.primary,
                                borderRadius: 30,
                            }}
                        />
                        <View style={styles.signupRow}>
                            <Text style={[styles.loginTitle, { color: dark ? 'rgba(255,255,255,0.7)' : COLORS.grayscale700 }]}>
                                Don't have an account?{' '}
                            </Text>
                            <TouchableOpacity onPress={() => navigate('signup')}>
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
    signupRow: {
        flexDirection: 'row',
        justifyContent: 'center',
    },
    loginTitle: {
        fontSize: 14,
        fontFamily: 'regular',
    },
    loginSubtitle: {
        fontSize: 14,
        fontFamily: 'bold',
    },
});

export default Welcome;
