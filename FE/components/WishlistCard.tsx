import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, Pressable, Image, ImageSourcePropType, GestureResponderEvent } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useTheme } from '../theme/ThemeProvider';
import { FontAwesome } from '@expo/vector-icons';
import { BlurView } from 'expo-blur';
import { LinearGradient } from 'expo-linear-gradient';
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withSpring,
    withSequence,
} from 'react-native-reanimated';
import * as Haptics from 'expo-haptics';

interface WishlistCardProps {
    name: string;
    image: ImageSourcePropType;
    numSolds: number;
    price: number | string;
    rating: number;
    onPress: (event: GestureResponderEvent) => void;
    onRemove?: () => void;
}

const CARD_W = (SIZES.width - 32) / 2 - 12;

const WishlistCard: React.FC<WishlistCardProps> = ({ name, image, numSolds, price, rating, onPress, onRemove }) => {
    const { dark } = useTheme();

    const scale = useSharedValue(1);
    const heartScale = useSharedValue(1);

    const cardAnimStyle = useAnimatedStyle(() => ({
        transform: [{ scale: scale.value }],
    }));

    const heartAnimStyle = useAnimatedStyle(() => ({
        transform: [{ scale: heartScale.value }],
    }));

    const handlePressIn = () => {
        scale.value = withSpring(0.95, { damping: 15, stiffness: 200 });
    };

    const handlePressOut = () => {
        scale.value = withSpring(1, { damping: 15, stiffness: 200 });
    };

    const handleRemove = () => {
        heartScale.value = withSequence(
            withSpring(1.45, { damping: 6, stiffness: 400 }),
            withSpring(0.7, { damping: 10, stiffness: 200 })
        );
        Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium);
        setTimeout(() => onRemove?.(), 220);
    };

    return (
        <Animated.View style={[
            styles.outerContainer,
            cardAnimStyle,
            { shadowOpacity: dark ? 0.4 : 0.14 },
        ]}>
            <Pressable
                onPress={onPress}
                onPressIn={handlePressIn}
                onPressOut={handlePressOut}
                style={styles.pressable}
            >
                <View style={styles.innerContainer}>
                    <Image source={image} resizeMode="cover" style={styles.cardImage} />
                    <LinearGradient
                        colors={['transparent', 'rgba(0,0,0,0.06)', 'rgba(0,0,0,0.65)', 'rgba(0,0,0,0.90)']}
                        locations={[0, 0.42, 0.76, 1]}
                        style={StyleSheet.absoluteFillObject}
                    />
                    <View style={styles.heartWrapper}>
                        <BlurView intensity={70} tint="dark" style={styles.blurHeart}>
                            <Pressable onPress={handleRemove} style={styles.heartTouch} hitSlop={8}>
                                <Animated.View style={heartAnimStyle}>
                                    <FontAwesome name="heart" size={13} color="#FF4B4B" />
                                </Animated.View>
                            </Pressable>
                        </BlurView>
                    </View>
                    <View style={styles.bottomContent}>
                        <Text numberOfLines={1} style={styles.name}>{name}</Text>
                        <View style={styles.metaRow}>
                            <View style={styles.ratingBadge}>
                                <FontAwesome name="star" size={9} color="#FACC15" />
                                <Text style={styles.ratingText}> {rating}</Text>
                            </View>
                            <View style={styles.priceBadge}>
                                <Text style={styles.priceText}>${price}</Text>
                            </View>
                        </View>
                    </View>
                </View>
            </Pressable>
        </Animated.View>
    );
};

const styles = StyleSheet.create({
    outerContainer: {
        width: CARD_W,
        borderRadius: 20,
        marginBottom: 16,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 6 },
        shadowOpacity: 0.14,
        shadowRadius: 14,
        elevation: 8,
    },
    pressable: {},
    innerContainer: {
        width: CARD_W,
        height: 210,
        borderRadius: 20,
        overflow: 'hidden',
        backgroundColor: '#1a1a1a',
    },
    cardImage: {
        width: CARD_W,
        height: 210,
        position: 'absolute',
        top: 0,
        left: 0,
    },
    heartWrapper: {
        position: 'absolute',
        top: 10,
        right: 10,
        width: 34,
        height: 34,
        borderRadius: 17,
        overflow: 'hidden',
        zIndex: 10,
    },
    blurHeart: { flex: 1, alignItems: 'center', justifyContent: 'center' },
    heartTouch: { width: 34, height: 34, alignItems: 'center', justifyContent: 'center' },
    bottomContent: { position: 'absolute', bottom: 12, left: 12, right: 12 },
    name: {
        fontSize: 13,
        fontFamily: 'bold',
        color: '#FFFFFF',
        marginBottom: 7,
        textShadowColor: 'rgba(0,0,0,0.6)',
        textShadowOffset: { width: 0, height: 1 },
        textShadowRadius: 4,
    },
    metaRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
    ratingBadge: {
        flexDirection: 'row',
        alignItems: 'center',
        backgroundColor: 'rgba(255,255,255,0.18)',
        paddingHorizontal: 8,
        paddingVertical: 3,
        borderRadius: 10,
        borderWidth: 0.5,
        borderColor: 'rgba(255,255,255,0.28)',
    },
    ratingText: { fontSize: 11, fontFamily: 'semiBold', color: '#FFFFFF' },
    priceBadge: { backgroundColor: COLORS.primary, paddingHorizontal: 10, paddingVertical: 4, borderRadius: 10 },
    priceText: { fontSize: 12, fontFamily: 'bold', color: '#FFFFFF' },
});

export default WishlistCard;
