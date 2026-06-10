import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, Image, TouchableOpacity, Platform, ImageSourcePropType } from 'react-native';
import { COLORS } from '../constants';
import { useTheme } from '../theme/ThemeProvider';

type PaymentMethodItemConnectedProps = {
    onPress: () => void;
    title: string;
    icon: ImageSourcePropType;
    tintColor?: string;
};

const PaymentMethodItemConnected: React.FC<PaymentMethodItemConnectedProps> = ({
    onPress,
    title,
    icon,
    tintColor,
}) => {
    const { dark } = useTheme();

    return (
        <TouchableOpacity
            onPress={onPress}
            style={[
                styles.container,
                Platform.OS === 'android' ? styles.androidShadow : styles.iosShadow,
                { backgroundColor: dark ? COLORS.dark2 : COLORS.white },
            ]}
        >
            <View style={styles.rightContainer}>
                <Image
                    source={icon}
                    resizeMode="contain"
                    style={[styles.icon, { tintColor: tintColor }]}
                />
                <View>
                    <Text style={[styles.title, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>{title}</Text>
                </View>
            </View>
            <View style={styles.leftContainer}>
                <Text style={[styles.connectedTitle, { color: dark ? COLORS.white : COLORS.primary }]}>Connected</Text>
            </View>
        </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
    container: {
        width: '100%',
        borderRadius: 16,
        paddingVertical: 12,
        paddingHorizontal: 10,
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 12,
        height: 76,
        backgroundColor: COLORS.white,
        shadowColor: COLORS.black,
        shadowOffset: {
            width: 0,
            height: 2,
        },
        shadowOpacity: 0.2,
        shadowRadius: 2,
        elevation: 1,
    },
    rightContainer: {
        flexDirection: 'row',
        alignItems: 'center',
    },
    icon: {
        height: 26,
        width: 26,
        marginRight: 16,
    },
    title: {
        fontSize: 18,
        fontFamily: 'bold',
        color: COLORS.greyscale900,
    },
    leftContainer: {
        flexDirection: 'row',
        alignItems: 'center',
    },
    androidShadow: {
        elevation: 1,
    },
    iosShadow: {
        shadowColor: 'rgba(4, 6, 15, 0.05)',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.8,
        shadowRadius: 4,
    },
    connectedTitle: {
        fontSize: 16,
        fontFamily: 'semiBold',
        color: COLORS.primary,
    },
});

export default PaymentMethodItemConnected;