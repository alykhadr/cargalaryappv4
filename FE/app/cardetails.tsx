import { View, TouchableOpacity, Image, Alert, ActivityIndicator, StyleSheet, Pressable } from 'react-native';
import React, { useEffect, useRef, useState } from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, SIZES, icons, socials } from '../constants';
import { BlurView } from 'expo-blur';
import RBSheet from "react-native-raw-bottom-sheet";
import { ScrollView } from 'react-native-virtualized-view';
import { StatusBar } from 'expo-status-bar';
import { useTheme } from '../theme/ThemeProvider';
import { FontAwesome } from "@expo/vector-icons";
import { NavigationProp, useNavigation } from '@react-navigation/native';
import { useLocalSearchParams } from 'expo-router';
import AutoSlider from '@/components/AutoSlider';
import SocialIcon from '@/components/SocialIcon';
import { api } from '@/services/api';
import { Car } from '@/types';
import { resolveCarImage, resolveBrandLogo, resolveSliderImages } from '@/utils/imageResolver';
import { useAuth } from '@/context/AuthContext';
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withTiming,
    withSpring,
    withSequence,
    Easing,
} from 'react-native-reanimated';
import * as Haptics from 'expo-haptics';

const CarDetails = () => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { carId } = useLocalSearchParams<{ carId: string }>();
    const [car, setCar] = useState<Car | null>(null);
    const [loading, setLoading] = useState(true);
    const [isFavorite, setIsFavorite] = useState(false);
    const [selectedColor, setSelectedColor] = useState<string | null>(null);
    const { dark } = useTheme();
    const refRBSheet = useRef<any>(null);
    const { user, isGuest } = useAuth();

    // Entrance animation for content
    const contentOpacity = useSharedValue(0);
    const contentY = useSharedValue(32);
    const btnScale = useSharedValue(1);

    const contentStyle = useAnimatedStyle(() => ({
        opacity: contentOpacity.value,
        transform: [{ translateY: contentY.value }],
    }));

    const btnAnimStyle = useAnimatedStyle(() => ({
        transform: [{ scale: btnScale.value }],
    }));

    useEffect(() => {
        if (!carId) return;
        setLoading(true);
        api.getCar(carId)
            .then(res => {
                setCar(res.car);
                if (res.car.colors?.length) setSelectedColor(res.car.colors[0]);
            })
            .catch(() => {})
            .finally(() => {
                setLoading(false);
                // Animate content in after load
                contentOpacity.value = withTiming(1, { duration: 480, easing: Easing.out(Easing.cubic) });
                contentY.value = withTiming(0, { duration: 480, easing: Easing.out(Easing.cubic) });
            });
    }, [carId]);

    useEffect(() => {
        if (!carId || !user) return;
        api.getWishlistStatus(carId)
            .then(res => setIsFavorite(res.isWishlisted))
            .catch(() => {});
    }, [carId, user]);

    const handleFavoriteToggle = async () => {
        if (isGuest || !user) {
            Alert.alert(
                'Login Required',
                'Please login to add cars to your wishlist',
                [
                    { text: 'Cancel', style: 'cancel' },
                    { text: 'Login', onPress: () => navigation.navigate('login') },
                ]
            );
            return;
        }
        try {
            if (isFavorite) {
                await api.removeFromWishlist(carId!);
            } else {
                await api.addToWishlist(carId!);
            }
            setIsFavorite(!isFavorite);
        } catch (e: any) {
            Alert.alert('Error', e.message);
        }
    };

    const handleInquire = () => {
        if (isGuest || !user) {
            Alert.alert(
                'Login Required',
                'Please login to contact the dealer',
                [
                    { text: 'Cancel', style: 'cancel' },
                    { text: 'Login', onPress: () => navigation.navigate('login') },
                ]
            );
            return;
        }
        navigation.navigate('makeoffer', { carId: car?.id, carName: car?.name });
    };

    const sliderImages = car
        ? resolveSliderImages(car.sliderImageKeys)
        : [];

    if (loading) {
        return (
            <View style={[styles.area, { justifyContent: 'center', alignItems: 'center', backgroundColor: dark ? COLORS.dark1 : COLORS.white }]}>
                <ActivityIndicator size="large" color={COLORS.primary} />
            </View>
        );
    }

    const BRAND_COLORS: Record<string, string> = {
        Mercedes: '#335EF7', Tesla: '#FF981F', BMW: '#1A96F0',
        Toyota: '#FFC02D', Volvo: '#F54336', Bugatti: '#4AAF57', Honda: '#00BCD3',
    };
    const brandAccent = BRAND_COLORS[car?.brand ?? ''] ?? COLORS.primary;

    const renderHeader = () => (
        <View style={styles.headerContainer}>
            <View style={styles.blurWrap}>
                <BlurView intensity={50} tint="dark" style={styles.blurInner}>
                    <TouchableOpacity onPress={() => navigation.goBack()} style={styles.iconBtn}>
                        <Image source={icons.back} resizeMode='contain' style={styles.headerIcon} />
                    </TouchableOpacity>
                </BlurView>
            </View>
            <View style={styles.iconContainer}>
                <View style={styles.blurWrap}>
                    <BlurView intensity={50} tint="dark" style={styles.blurInner}>
                        <TouchableOpacity onPress={handleFavoriteToggle} style={styles.iconBtn}>
                            <Image
                                source={isFavorite ? icons.heart2 : icons.heart2Outline}
                                resizeMode='contain'
                                style={[styles.headerIcon, { tintColor: isFavorite ? '#FF4B4B' : COLORS.white }]}
                            />
                        </TouchableOpacity>
                    </BlurView>
                </View>
                <View style={[styles.blurWrap, { marginLeft: 8 }]}>
                    <BlurView intensity={50} tint="dark" style={styles.blurInner}>
                        <TouchableOpacity onPress={() => refRBSheet.current.open()} style={styles.iconBtn}>
                            <Image source={icons.send2} resizeMode='contain' style={[styles.headerIcon, { tintColor: COLORS.white }]} />
                        </TouchableOpacity>
                    </BlurView>
                </View>
            </View>
        </View>
    );

    const renderContent = () => (
        <View style={styles.contentContainer}>
            <View style={styles.contentView}>
                <Text style={[styles.contentTitle, { color: dark ? COLORS.white : COLORS.black }]}>
                    {car?.name || ''}
                </Text>
                <TouchableOpacity onPress={handleFavoriteToggle}>
                    <Image
                        source={isFavorite ? icons.heart2 : icons.heart2Outline}
                        resizeMode='contain'
                        style={[styles.bookmarkIcon, { tintColor: dark ? COLORS.white : COLORS.black }]}
                    />
                </TouchableOpacity>
            </View>
            <View style={styles.ratingContainer}>
                <View style={[styles.ratingView, { backgroundColor: dark ? COLORS.dark3 : COLORS.silver }]}>
                    <Text style={[styles.ratingViewTitle, { color: dark ? COLORS.white : "#35383F" }]}>
                        {car?.numSolds?.toLocaleString() || '0'} sold
                    </Text>
                </View>
                <TouchableOpacity
                    onPress={() => navigation.navigate("productreviews")}
                    style={styles.starContainer}>
                    <Image
                        source={icons.starHalf2}
                        resizeMode='contain'
                        style={[styles.starIcon, { tintColor: dark ? COLORS.white : COLORS.black }]}
                    />
                    <Text style={[styles.reviewText, { color: dark ? COLORS.white : COLORS.greyScale800 }]}>
                        {"   "}{car?.rating} ({car?.numReviews} reviews)
                    </Text>
                </TouchableOpacity>
            </View>
            <View style={[styles.separateLine, { backgroundColor: dark ? COLORS.greyscale900 : COLORS.grayscale200 }]} />
            <Text style={[styles.descTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>Description</Text>
            <Text style={[styles.descText, { color: dark ? COLORS.greyscale300 : COLORS.greyScale800 }]}>
                {car?.description || ''}
            </Text>
            <View style={styles.featureContainer}>
                <View style={styles.featureView}>
                    <Text style={[styles.descTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>Color</Text>
                    <View style={styles.colorContainer}>
                        {(car?.colors || []).map((color) => (
                            <TouchableOpacity
                                key={color}
                                style={[
                                    styles.colorView,
                                    { backgroundColor: color },
                                    selectedColor === color && styles.selectedColor,
                                ]}
                                onPress={() => setSelectedColor(color)}
                            >
                                {selectedColor === color && <FontAwesome name="check" size={18} color="white" />}
                            </TouchableOpacity>
                        ))}
                    </View>
                </View>
            </View>
            <View style={[styles.separateLine, { backgroundColor: dark ? COLORS.greyscale900 : COLORS.grayscale200 }]} />
            <View style={styles.companyCardContainer}>
                <View style={styles.companyCardLeft}>
                    <Image
                        source={resolveBrandLogo(car?.brandLogoKey || '')}
                        resizeMode='contain'
                        style={[styles.companyLogo, { tintColor: dark ? COLORS.white : COLORS.black }]}
                    />
                    <View>
                        <View style={styles.premiumIconContainer}>
                            <Text style={[styles.companyName, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
                                {car?.brand} Store
                            </Text>
                            <Image source={icons.premium} resizeMode='contain' style={styles.premiumIcon} />
                        </View>
                        <Text style={[styles.companyDesc, { color: dark ? COLORS.grayscale100 : COLORS.grayscale700 }]}>
                            Official {car?.brand} dealer
                        </Text>
                    </View>
                </View>
            </View>
        </View>
    );

    return (
        <View style={[styles.area, { backgroundColor: dark ? COLORS.dark1 : COLORS.white }]}>
            <StatusBar hidden />
            <AutoSlider images={sliderImages} />
            {renderHeader()}
            <ScrollView showsVerticalScrollIndicator={false}>
                <Animated.View style={contentStyle}>
                    {renderContent()}
                </Animated.View>
            </ScrollView>
            <View style={[styles.cartBottomContainer, {
                backgroundColor: dark ? COLORS.dark1 : COLORS.white,
                borderTopColor: dark ? COLORS.dark1 : COLORS.white,
                shadowColor: '#000',
                shadowOffset: { width: 0, height: -3 },
                shadowOpacity: dark ? 0.25 : 0.07,
                shadowRadius: 12,
                elevation: 12,
            }]}>
                <View style={{ borderLeftWidth: 3, borderLeftColor: brandAccent, paddingLeft: 10 }}>
                    <Text style={[styles.cartTitle, { color: dark ? COLORS.greyscale300 : COLORS.greyscale600 }]}>
                        Price
                    </Text>
                    <Text style={[styles.cartSubtitle, { color: dark ? COLORS.white : COLORS.black }]}>
                        ${car?.price || ''}
                    </Text>
                </View>
                <Animated.View style={btnAnimStyle}>
                    <Pressable
                        onPress={() => {
                            Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium);
                            handleInquire();
                        }}
                        onPressIn={() => {
                            btnScale.value = withSpring(0.94, { damping: 12, stiffness: 260 });
                        }}
                        onPressOut={() => {
                            btnScale.value = withSpring(1, { damping: 12, stiffness: 260 });
                        }}
                        style={[styles.cartBtn, { backgroundColor: dark ? COLORS.white : COLORS.black }]}>
                        <Text style={[styles.cartBtnText, { color: dark ? COLORS.black : COLORS.white }]}>
                            Contact Dealer
                        </Text>
                    </Pressable>
                </Animated.View>
            </View>
            <RBSheet
                ref={refRBSheet}
                closeOnPressMask={true}
                height={360}
                customStyles={{
                    wrapper: { backgroundColor: "rgba(0,0,0,0.5)" },
                    draggableIcon: { backgroundColor: dark ? COLORS.dark3 : COLORS.grayscale200 },
                    container: {
                        borderTopRightRadius: 32,
                        borderTopLeftRadius: 32,
                        height: 360,
                        backgroundColor: dark ? COLORS.dark2 : COLORS.white,
                        alignItems: "center",
                    }
                }}
            >
                <Text style={[styles.bottomTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>Share</Text>
                <View style={[styles.separateLine, {
                    backgroundColor: dark ? COLORS.grayscale700 : COLORS.grayscale200,
                    marginVertical: 12
                }]} />
                <View style={styles.socialContainer}>
                    <SocialIcon icon={socials.whatsapp} name="WhatsApp" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.twitter} name="X" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.facebook} name="Facebook" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.instagram} name="Instagram" onPress={() => refRBSheet.current.close()} />
                </View>
                <View style={styles.socialContainer}>
                    <SocialIcon icon={socials.yahoo} name="Yahoo" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.titktok} name="Tiktok" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.messenger} name="Chat" onPress={() => refRBSheet.current.close()} />
                    <SocialIcon icon={socials.wechat} name="Wechat" onPress={() => refRBSheet.current.close()} />
                </View>
            </RBSheet>
        </View>
    );
}

const styles = StyleSheet.create({
    area: { flex: 1, backgroundColor: COLORS.white },
    headerContainer: {
        width: SIZES.width - 32,
        flexDirection: "row",
        justifyContent: "space-between",
        position: "absolute",
        top: 32,
        zIndex: 999,
        left: 16,
        right: 16
    },
    blurWrap: { borderRadius: 20, overflow: 'hidden' },
    blurInner: { borderRadius: 20 },
    iconBtn: { width: 36, height: 36, alignItems: 'center', justifyContent: 'center' },
    headerIcon: { width: 20, height: 20, tintColor: COLORS.white },
    bookmarkIcon: { width: 24, height: 24, tintColor: COLORS.black },
    iconContainer: { flexDirection: "row", alignItems: "center" },
    contentContainer: { marginHorizontal: 12 },
    bottomTitle: {
        fontSize: 24,
        fontFamily: "semiBold",
        color: COLORS.black,
        textAlign: "center",
        marginTop: 12
    },
    socialContainer: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        marginVertical: 12,
        width: SIZES.width - 32
    },
    contentView: {
        alignItems: "center",
        justifyContent: "space-between",
        flexDirection: "row",
        width: SIZES.width - 32
    },
    contentTitle: { fontSize: 30, fontFamily: "bold", color: COLORS.black },
    ratingContainer: {
        flexDirection: "row",
        alignItems: "center",
        width: SIZES.width - 32,
        marginVertical: 12
    },
    ratingView: {
        paddingHorizontal: 8,
        height: 24,
        borderRadius: 4,
        backgroundColor: COLORS.silver,
        alignItems: "center",
        justifyContent: "center"
    },
    ratingViewTitle: { fontSize: 10, fontFamily: "semiBold", color: "#35383F", textAlign: "center" },
    starContainer: { marginHorizontal: 16, flexDirection: "row", alignItems: "center" },
    starIcon: { height: 20, width: 20 },
    reviewText: { fontSize: 14, fontFamily: "medium", color: COLORS.greyScale800 },
    separateLine: { width: "100%", height: 1, backgroundColor: COLORS.grayscale200 },
    descTitle: { fontSize: 18, fontFamily: "bold", color: COLORS.greyscale900, marginVertical: 8 },
    descText: { fontSize: 14, color: COLORS.greyScale800, fontFamily: "regular" },
    featureContainer: {
        flexDirection: "row",
        alignItems: "center",
        width: SIZES.width - 32,
        marginVertical: 12
    },
    featureView: { flexDirection: "column" },
    colorContainer: { flexDirection: 'row', alignItems: 'center' },
    colorView: {
        width: 40,
        height: 40,
        borderRadius: 25,
        justifyContent: 'center',
        alignItems: 'center',
        marginRight: 8
    },
    selectedColor: { marginRight: 7.8 },
    cartBottomContainer: {
        position: "absolute",
        bottom: 12,
        left: 0,
        right: 0,
        width: SIZES.width,
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        height: 104,
        backgroundColor: COLORS.white,
        paddingHorizontal: 16,
        paddingVertical: 16,
        borderTopRightRadius: 32,
        borderTopLeftRadius: 32,
        borderTopColor: COLORS.white,
        borderTopWidth: 1,
    },
    cartTitle: { fontSize: 12, fontFamily: "medium", color: COLORS.greyscale600, marginBottom: 6 },
    cartSubtitle: { fontSize: 24, fontFamily: "bold", color: COLORS.black },
    cartBtn: {
        height: 58,
        width: 200,
        alignItems: "center",
        justifyContent: "center",
        borderRadius: 32,
        backgroundColor: COLORS.black,
        flexDirection: "row",
    },
    cartBtnText: { fontSize: 16, fontFamily: "bold", color: COLORS.white, textAlign: "center" },
    companyCardContainer: {
        flexDirection: "row",
        alignItems: "center",
        width: SIZES.width - 32,
        marginVertical: 12,
        justifyContent: "space-between"
    },
    companyCardLeft: { flexDirection: "row", alignItems: "center" },
    companyLogo: { height: 60, width: 60, marginRight: 22 },
    companyName: { fontSize: 18, color: COLORS.black, fontFamily: "bold" },
    companyDesc: { fontSize: 14, fontFamily: "medium", color: COLORS.grayscale700, marginTop: 4 },
    premiumIcon: { height: 14, width: 14, tintColor: "#5F82FF", marginHorizontal: 8 },
    premiumIconContainer: { flexDirection: "row", alignItems: "center" },
})

export default CarDetails
