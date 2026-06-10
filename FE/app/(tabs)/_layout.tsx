import React, { useEffect } from 'react';
import Text from '@/components/LocalizedText';
import { useTheme } from "@/theme/ThemeProvider";
import { Tabs } from "expo-router";
import { Image, Platform, StyleSheet, View } from "react-native";
import { COLORS, icons, SIZES } from "../../constants";
import { BlurView } from 'expo-blur';
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withSpring,
    withSequence,
} from 'react-native-reanimated';

const TabItem = ({ focused, dark, icon, focusedIcon, label }: {
    focused: boolean; dark: boolean;
    icon: any; focusedIcon: any; label: string;
}) => {
    const pillScale = useSharedValue(focused ? 1 : 0);
    const pillOpacity = useSharedValue(focused ? 1 : 0);
    const iconScale = useSharedValue(1);

    useEffect(() => {
        pillScale.value = withSpring(focused ? 1 : 0, { damping: 14, stiffness: 200 });
        pillOpacity.value = withSpring(focused ? 1 : 0, { damping: 14, stiffness: 200 });
        if (focused) {
            iconScale.value = withSequence(
                withSpring(1.22, { damping: 7, stiffness: 380 }),
                withSpring(1, { damping: 13, stiffness: 200 })
            );
        }
    }, [focused]);

    const pillStyle = useAnimatedStyle(() => ({
        transform: [{ scale: pillScale.value }],
        opacity: pillOpacity.value,
    }));

    const iconContainerStyle = useAnimatedStyle(() => ({
        transform: [{ scale: iconScale.value }],
    }));

    return (
        <View style={styles.tabItem}>
            <Animated.View style={[
                styles.pill,
                pillStyle,
                { backgroundColor: dark ? 'rgba(255,255,255,0.12)' : 'rgba(16,16,16,0.08)' },
            ]} />
            <Animated.View style={iconContainerStyle}>
                <Image
                    source={focused ? focusedIcon : icon}
                    resizeMode="contain"
                    style={{
                        width: 22,
                        height: 22,
                        tintColor: focused
                            ? dark ? COLORS.white : COLORS.primary
                            : dark ? COLORS.gray3 : COLORS.gray3,
                        marginBottom: 2,
                    }}
                />
            </Animated.View>
            <Text style={{
                fontSize: 10,
                fontFamily: focused ? 'semiBold' : 'regular',
                color: focused
                    ? dark ? COLORS.white : COLORS.primary
                    : dark ? COLORS.gray3 : COLORS.gray3,
            }}>{label}</Text>
        </View>
    );
};

const TabLayout = () => {
    const { dark } = useTheme();

    return (
        <Tabs
            screenOptions={{
                headerShown: false,
                tabBarHideOnKeyboard: Platform.OS !== 'ios',
                tabBarStyle: {
                    position: 'absolute',
                    bottom: 0,
                    right: 0,
                    left: 0,
                    height: Platform.OS === 'ios' ? 88 : 64,
                    backgroundColor: dark ? 'rgba(0,0,0,0.85)' : 'rgba(255,255,255,0.92)',
                    borderTopWidth: 0.5,
                    borderTopColor: dark ? 'rgba(255,255,255,0.08)' : 'rgba(0,0,0,0.06)',
                    elevation: 0,
                },
                tabBarBackground: () => (
                    <BlurView
                        intensity={Platform.OS === 'ios' ? 80 : 0}
                        tint={dark ? 'dark' : 'light'}
                        style={StyleSheet.absoluteFill}
                    />
                ),
            }}
        >
            <Tabs.Screen
                name="index"
                options={{
                    title: "",
                    tabBarIcon: ({ focused }) => (
                        <TabItem focused={focused} dark={dark}
                            icon={icons.home2Outline} focusedIcon={icons.home}
                            label="Home" />
                    ),
                }}
            />
            <Tabs.Screen
                name="orders"
                options={{
                    title: "",
                    tabBarIcon: ({ focused }) => (
                        <TabItem focused={focused} dark={dark}
                            icon={icons.chatOutline} focusedIcon={icons.chat}
                            label="Inquiries" />
                    ),
                }}
            />
            <Tabs.Screen name="inbox" options={{ href: null }} />
            <Tabs.Screen name="wallet" options={{ href: null }} />
            <Tabs.Screen
                name="profile"
                options={{
                    title: "",
                    tabBarIcon: ({ focused }) => (
                        <TabItem focused={focused} dark={dark}
                            icon={icons.profileOutline} focusedIcon={icons.profile}
                            label="Profile" />
                    ),
                }}
            />
        </Tabs>
    );
};

const styles = StyleSheet.create({
    tabItem: {
        alignItems: 'center',
        paddingTop: 12,
        width: SIZES.width / 3,
    },
    pill: {
        position: 'absolute',
        top: 8,
        width: 52,
        height: 32,
        borderRadius: 16,
    },
});

export default TabLayout;
