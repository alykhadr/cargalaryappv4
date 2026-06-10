import React from 'react';
import Text from '@/components/LocalizedText';
import { TouchableOpacity, StyleSheet, ActivityIndicator, ViewStyle, StyleProp } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useTheme } from '../theme/ThemeProvider';

interface ButtonOutlinedProps {
    onPress: () => void;
    title: string;
    style?: StyleProp<ViewStyle>;
    isLoading?: boolean;
}

const ButtonOutlined: React.FC<ButtonOutlinedProps> = ({
    onPress,
    title,
    style,
    isLoading = false,
}) => {
    const { dark } = useTheme();

    return (
        <TouchableOpacity
            style={[
                styles.filledButton,
                style,
                {
                    backgroundColor: 'transparent',
                    borderColor: dark ? COLORS.white : COLORS.primary,
                },
            ]}
            onPress={onPress}
        >
            {isLoading ? (
                <ActivityIndicator size="small" color={COLORS.white} />
            ) : (
                <Text
                    style={{
                        fontSize: 18,
                        fontFamily: 'semiBold',
                        color: dark ? COLORS.white : COLORS.black,
                    }}
                >
                    {title}
                </Text>
            )}
        </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
    filledButton: {
        paddingHorizontal: SIZES.padding,
        paddingVertical: SIZES.padding,
        borderColor: COLORS.primary,
        borderWidth: 1,
        borderRadius: 25,
        alignItems: 'center',
        justifyContent: 'center',
        height: 52,
        backgroundColor: COLORS.primary,
    },
});

export default ButtonOutlined;