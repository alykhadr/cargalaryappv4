import { useNavigation } from '@react-navigation/native';
import { LinearGradient } from 'expo-linear-gradient';
import React, { useEffect } from 'react';
import Text from '@/components/LocalizedText';
import { ImageBackground, StyleSheet } from 'react-native';
import { COLORS, images } from '../constants';

type Nav = {
    navigate: (value: string) => void
}

const Onboarding1 = () => {
    const { navigate } = useNavigation<Nav>();
    // Add useEffect
    useEffect(() => {
        const timeout = setTimeout(() => {
            navigate('welcome');
        }, 2000);

        return () => clearTimeout(timeout);
    }, []); // run only once after component mounts

    return (
        <ImageBackground
            source={images.splashOnboarding}
            style={styles.area}>
            <LinearGradient
                // Background linear gradient
                colors={['transparent', 'rgba(0,0,0,0.8)']}
                style={styles.background}>
                <Text style={styles.greetingText}>Welcome to 👋</Text>
                <Text style={styles.logoName}>Carea</Text>
                <Text style={styles.subtitle}>The best car marketplace app of the century for your transportation needs!</Text>
            </LinearGradient>
        </ImageBackground>
    )
};

const styles = StyleSheet.create({
    area: {
        flex: 1
    },
    background: {
        position: 'absolute',
        bottom: 0,
        width: '100%',
        height: 270,
        paddingHorizontal: 16
    },
    greetingText: {
        fontSize: 40,
        color: COLORS.white,
        fontFamily: 'bold',
        marginVertical: 12
    },
    logoName: {
        fontSize: 76,
        color: COLORS.white,
        fontFamily: 'extraBold',
    },
    subtitle: {
        fontSize: 16,
        color: COLORS.white,
        marginVertical: 12,
        fontFamily: "semiBold",
    }
})

export default Onboarding1;