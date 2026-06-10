import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, TouchableOpacity } from 'react-native';
import { COLORS } from '../constants';
import { AppDirection, useDirection } from '../theme/DirectionProvider';
import { useTheme } from '../theme/ThemeProvider';

type LanguageItemProps = {
    checked: boolean;
    direction?: AppDirection;
    name: string;
    onPress: () => void;
};

const LanguageItem: React.FC<LanguageItemProps> = ({ checked, direction, name, onPress }) => {
    const { dark } = useTheme();
    const { textDirectionStyle } = useDirection();

    return (
        <TouchableOpacity onPress={onPress} style={styles.container}>
            <View style={styles.nameContainer}>
                <Text style={[
                    styles.name,
                    textDirectionStyle,
                    { color: dark ? COLORS.white : COLORS.black },
                ]}>
                    {name}
                </Text>
                {direction && (
                    <Text style={[
                        styles.direction,
                        {
                            backgroundColor: dark ? COLORS.dark3 : COLORS.tansparentPrimary,
                            color: dark ? COLORS.white : COLORS.primary,
                        },
                    ]}>
                        {direction.toUpperCase()}
                    </Text>
                )}
            </View>
            <View
                style={[
                    styles.roundedChecked,
                    { borderColor: dark ? COLORS.white : COLORS.primary },
                ]}
            >
                {checked && (
                    <View
                        style={[
                            styles.roundedCheckedCheck,
                            { backgroundColor: dark ? COLORS.white : COLORS.primary },
                        ]}
                    />
                )}
            </View>
        </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        marginVertical: 6,
    },
    nameContainer: {
        flex: 1,
        flexDirection: 'row',
        alignItems: 'center',
    },
    name: {
        fontSize: 18,
        fontFamily: 'medium',
        color: COLORS.black,
        marginBottom: 10,
        flexShrink: 1,
    },
    direction: {
        overflow: 'hidden',
        borderRadius: 999,
        fontSize: 12,
        fontFamily: 'semiBold',
        paddingHorizontal: 10,
        paddingVertical: 4,
        marginStart: 10,
        marginBottom: 10,
    },
    roundedChecked: {
        width: 20,
        height: 20,
        borderRadius: 15,
        borderWidth: 2,
        borderColor: COLORS.primary,
        alignItems: 'center',
        justifyContent: 'center',
        marginEnd: 10,
    },
    roundedCheckedCheck: {
        height: 10,
        width: 10,
        backgroundColor: COLORS.primary,
        borderRadius: 999,
    },
});

export default LanguageItem;
