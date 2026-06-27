import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, TouchableOpacity, Image, ImageSourcePropType, GestureResponderEvent } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useTheme } from '../theme/ThemeProvider';

interface CategoryProps {
    name: string;
    icon: ImageSourcePropType;
    iconColor?: string;
    backgroundColor?: string;
    preserveIconColor?: boolean;
    onPress?: () => void;
}

const Category: React.FC<CategoryProps> = ({
    name,
    icon,
    iconColor,
    backgroundColor,
    preserveIconColor = false,
    onPress
}) => {
    const { dark } = useTheme();

    return (
        <View style={styles.container}>
            <TouchableOpacity
                activeOpacity={0.82}
                onPress={onPress}
                style={[
                    styles.iconContainer,
                    { backgroundColor: backgroundColor ?? (dark ? COLORS.dark3 : COLORS.silver) }
                ]}>
                <Image
                    source={icon}
                    resizeMode="contain"
                    style={[
                        styles.icon,
                        preserveIconColor
                            ? null
                            : { tintColor: iconColor ?? (dark ? COLORS.white : COLORS.greyscale900) }
                    ]}
                />
            </TouchableOpacity>
            <Text style={[
                styles.name,
                { color: dark ? COLORS.white : COLORS.greyscale900 }
            ]}
                numberOfLines={1}>
                {name}
            </Text>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flexDirection: "column",
        alignItems: "center",
        marginBottom: 12,
        width: (SIZES.width - 32) / 4
    },
    iconContainer: {
        width: 54,
        height: 54,
        borderRadius: 999,
        justifyContent: "center",
        alignItems: "center",
        marginBottom: 8
    },
    icon: {
        height: 24,
        width: 24
    },
    name: {
        fontSize: 13,
        fontFamily: "medium",
        color: COLORS.black,
        textAlign: 'center'
    }
});

export default Category;
