import React, { useState, useEffect, useRef } from 'react';
import { View, Image, StyleSheet, ScrollView, Dimensions, NativeSyntheticEvent, NativeScrollEvent } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useTheme } from '../theme/ThemeProvider';

interface AutoSliderProps {
    images: { uri: string }[];
}

const AutoSlider: React.FC<AutoSliderProps> = ({ images }) => {
    const [currentIndex, setCurrentIndex] = useState(0);
    const [isReadyToScroll, setIsReadyToScroll] = useState(false);
    const scrollViewRef = useRef<ScrollView>(null);
    const { dark } = useTheme();

    useEffect(() => {
        setCurrentIndex(0);
        setIsReadyToScroll(false);
    }, [images.length]);

    useEffect(() => {
        if (images.length < 2 || !isReadyToScroll) {
            return;
        }

        const interval = setInterval(() => {
            const nextIndex = (currentIndex + 1) % images.length;
            setCurrentIndex(nextIndex);
            requestAnimationFrame(() => {
                scrollViewRef.current?.scrollTo({
                    animated: true,
                    x: Dimensions.get('window').width * nextIndex,
                    y: 0,
                });
            });
        }, 3000); // Change slide every 3 seconds

        return () => clearInterval(interval);
    }, [currentIndex, images.length, isReadyToScroll]);

    if (images.length === 0) {
        return (
            <View style={[styles.container, styles.emptyContainer, { backgroundColor: dark ? COLORS.dark3 : COLORS.silver }]} />
        );
    }

    const handleScroll = (event: NativeSyntheticEvent<NativeScrollEvent>) => {
        const contentOffsetX = event.nativeEvent.contentOffset.x;
        const newIndex = Math.round(contentOffsetX / Dimensions.get('window').width);
        setCurrentIndex(newIndex);
    };

    const handlePaginationPress = (index: number) => {
        setCurrentIndex(index);
        if (isReadyToScroll) {
            scrollViewRef.current?.scrollTo({
                animated: true,
                x: Dimensions.get('window').width * index,
                y: 0,
            });
        }
    };

    return (
        <View style={[styles.container, { backgroundColor: dark ? COLORS.dark3 : COLORS.silver }]}>
            <ScrollView
                ref={scrollViewRef}
                horizontal
                pagingEnabled
                showsHorizontalScrollIndicator={false}
                onScroll={handleScroll}
                onContentSizeChange={() => setIsReadyToScroll(true)}
                scrollEventThrottle={16}
            >
                {images.map((image, index) => (
                    <Image
                        key={`${image.uri}-${index}`}
                        style={styles.image}
                        source={image}
                        resizeMode="cover"
                    />
                ))}
            </ScrollView>
            <View style={styles.pagination}>
                {images.map((_, index) => (
                    <View
                        key={index}
                        style={[
                            styles.paginationDot,
                            { backgroundColor: index === currentIndex ? COLORS.primary : COLORS.white },
                        ]}
                        onTouchStart={() => handlePaginationPress(index)}
                    />
                ))}
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        width: SIZES.width,
        height: SIZES.width * 0.9,
        backgroundColor: COLORS.silver,
    },
    emptyContainer: {
        justifyContent: 'center',
        alignItems: 'center',
    },
    image: {
        width: SIZES.width,
        height: SIZES.width * 0.9,
    },
    pagination: {
        flexDirection: 'row',
        position: 'absolute',
        bottom: 20,
        alignSelf: 'center',
    },
    paginationDot: {
        width: 8,
        height: 8,
        borderRadius: 4,
        marginHorizontal: 5,
    },
});

export default AutoSlider;
