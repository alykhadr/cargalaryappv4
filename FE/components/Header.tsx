import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, TouchableOpacity, Image } from 'react-native';
import { SIZES, COLORS, icons } from '../constants';
import { useNavigation } from '@react-navigation/native';
import { useDirection } from '../theme/DirectionProvider';
import { useTheme } from '../theme/ThemeProvider';

interface HeaderProps {
    title: string;
}

const Header: React.FC<HeaderProps> = ({ title }) => {
    const navigation = useNavigation();
    const { colors, dark } = useTheme();
    const { iconFlipStyle, textDirectionStyle } = useDirection();

    return (
        <View
            style={[
                styles.container,
                { backgroundColor: dark ? COLORS.dark1 : COLORS.white },
            ]}
        >
            <TouchableOpacity onPress={() => navigation.goBack()}>
                <Image
                    source={icons.back}
                    resizeMode="contain"
                    style={[
                        styles.backIcon,
                        iconFlipStyle,
                        { tintColor: colors.text },
                    ]}
                />
            </TouchableOpacity>
            <Text style={[styles.title, textDirectionStyle, { color: colors.text }]}>
                {title}
            </Text>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        backgroundColor: COLORS.white,
        width: SIZES.width - 32,
        flexDirection: 'row',
        alignItems: 'center',
    },
    backIcon: {
        width: 24,
        height: 24,
        marginEnd: 16,
    },
    title: {
        fontSize: 22,
        fontFamily: 'bold',
        color: COLORS.black,
    },
});

export default Header;
