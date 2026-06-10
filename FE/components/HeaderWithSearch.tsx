import React from 'react';
import Text from '@/components/LocalizedText';
import { View, TouchableOpacity, Image, StyleSheet, ImageSourcePropType, GestureResponderEvent } from 'react-native';
import { COLORS, SIZES, icons } from '../constants';
import { useDirection } from '../theme/DirectionProvider';
import { useTheme } from '../theme/ThemeProvider';
import { NavigationProp, useNavigation } from '@react-navigation/native';

interface HeaderWithSearchProps {
    title: string;
    icon: ImageSourcePropType;
    onPress: (event: GestureResponderEvent) => void;
}

const HeaderWithSearch: React.FC<HeaderWithSearchProps> = ({ title, icon, onPress }) => {
    const navigation = useNavigation<NavigationProp<any>>();
    const { dark } = useTheme();
    const { iconFlipStyle, textDirectionStyle } = useDirection();

    return (
        <View style={styles.headerContainer}>
            <View style={styles.headerLeft}>
                <TouchableOpacity onPress={() => navigation.goBack()}>
                    <Image
                        source={icons.back}
                        resizeMode="contain"
                        style={[
                            styles.backIcon,
                            iconFlipStyle,
                            { tintColor: dark ? COLORS.white : COLORS.greyscale900 }
                        ]}
                    />
                </TouchableOpacity>
                <Text style={[
                    styles.headerTitle,
                    textDirectionStyle,
                    { color: dark ? COLORS.white : COLORS.greyscale900 }
                ]}>
                    {title}
                </Text>
            </View>
            <TouchableOpacity onPress={onPress}>
                <Image
                    source={icon}
                    resizeMode="contain"
                    style={[
                        styles.moreIcon,
                        { tintColor: dark ? COLORS.white : COLORS.greyscale900 }
                    ]}
                />
            </TouchableOpacity>
        </View>
    );
};

const styles = StyleSheet.create({
    area: {
        flex: 1,
        backgroundColor: COLORS.white
    },
    container: {
        flex: 1,
        backgroundColor: COLORS.white,
        padding: 16
    },
    headerContainer: {
        flexDirection: "row",
        width: SIZES.width - 32,
        justifyContent: "space-between",
        marginBottom: 16
    },
    headerLeft: {
        flexDirection: "row",
        alignItems: "center"
    },
    backIcon: {
        height: 24,
        width: 24,
        tintColor: COLORS.black
    },
    headerTitle: {
        fontSize: 20,
        fontFamily: 'bold',
        color: COLORS.black,
        marginStart: 16
    },
    moreIcon: {
        width: 24,
        height: 24,
        tintColor: COLORS.black
    },
    searchBarContainer: {
        width: SIZES.width - 32,
        backgroundColor: COLORS.secondaryWhite,
        padding: 16,
        borderRadius: 12,
        height: 52,
        marginBottom: 16,
        flexDirection: "row",
        alignItems: "center"
    },
    searchIcon: {
        height: 24,
        width: 24,
        tintColor: COLORS.gray
    }
});

export default HeaderWithSearch;
