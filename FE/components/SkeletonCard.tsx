import React, { useEffect, useRef } from 'react';
import { Animated, View, StyleSheet } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { COLORS, SIZES } from '@/constants';
import { useTheme } from '@/theme/ThemeProvider';

const CARD_WIDTH = (SIZES.width - 32) / 2 - 12;

const ShimmerLine: React.FC<{ width: string | number; height: number; shimmer: Animated.Value; dark: boolean }> = ({ width, height, shimmer, dark }) => {
    const baseColor = dark ? '#2a2a2a' : '#E8E8E8';
    const shineColor = dark ? '#3d3d3d' : '#F5F5F5';
    const translateX = shimmer.interpolate({
        inputRange: [0, 1],
        outputRange: [-CARD_WIDTH, CARD_WIDTH],
    });
    return (
        <View style={[styles.shimmerLine, { width, height, backgroundColor: baseColor }]}>
            <Animated.View style={{ transform: [{ translateX }] }}>
                <LinearGradient
                    colors={['transparent', shineColor, 'transparent']}
                    start={{ x: 0, y: 0.5 }}
                    end={{ x: 1, y: 0.5 }}
                    style={{ width: CARD_WIDTH * 2, height }}
                />
            </Animated.View>
        </View>
    );
};

const SkeletonCard: React.FC = () => {
    const { dark } = useTheme();
    const shimmer = useRef(new Animated.Value(0)).current;

    useEffect(() => {
        Animated.loop(
            Animated.sequence([
                Animated.timing(shimmer, { toValue: 1, duration: 900, useNativeDriver: false }),
                Animated.timing(shimmer, { toValue: 0, duration: 900, useNativeDriver: false }),
            ])
        ).start();
    }, []);

    const baseColor = dark ? '#2a2a2a' : '#E8E8E8';

    return (
        <View style={[styles.container, { backgroundColor: dark ? COLORS.dark2 : COLORS.white }]}>
            <ShimmerLine width="100%" height={140} shimmer={shimmer} dark={dark} />
            <View style={{ height: 8 }} />
            <ShimmerLine width="75%" height={16} shimmer={shimmer} dark={dark} />
            <View style={{ height: 6 }} />
            <ShimmerLine width="50%" height={12} shimmer={shimmer} dark={dark} />
            <View style={{ height: 6 }} />
            <ShimmerLine width="40%" height={16} shimmer={shimmer} dark={dark} />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        width: CARD_WIDTH,
        borderRadius: 16,
        padding: 6,
        marginBottom: 12,
    },
    shimmerLine: {
        borderRadius: 8,
        overflow: 'hidden',
    },
});

export default SkeletonCard;
