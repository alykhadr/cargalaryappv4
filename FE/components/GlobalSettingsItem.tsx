import React from 'react';
import Text from '@/components/LocalizedText';
import { View, StyleSheet, Switch } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useDirection } from '../theme/DirectionProvider';
import { useTheme } from '../theme/ThemeProvider';

type GlobalSettingsItemProps = {
    title: string;
    isNotificationEnabled: boolean;
    toggleNotificationEnabled: () => void;
    showSwitch?: boolean;
};

const GlobalSettingsItem: React.FC<GlobalSettingsItemProps> = ({
    title,
    isNotificationEnabled,
    toggleNotificationEnabled,
    showSwitch = true,
}) => {
    const { dark } = useTheme();
    const { textDirectionStyle } = useDirection();

    return (
        <View style={styles.container}>
            <Text style={[
                styles.title,
                textDirectionStyle,
                { color: dark ? COLORS.tertiaryWhite : COLORS.greyscale900 },
            ]}>
                {title}
            </Text>
            {showSwitch && (
                <Switch
                    value={isNotificationEnabled}
                    onValueChange={toggleNotificationEnabled}
                    thumbColor={isNotificationEnabled ? '#fff' : COLORS.white}
                    trackColor={{ false: '#EEEEEE', true: dark ? COLORS.dark3 : COLORS.primary }}
                    ios_backgroundColor={COLORS.white}
                    style={styles.switch}
                />
            )}
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        width: SIZES.width - 32,
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginVertical: 12,
    },
    title: {
        fontSize: 18,
        fontFamily: 'semiBold',
        color: COLORS.blackTie,
    },
    switch: {
        marginStart: 8,
        transform: [{ scaleX: 0.8 }, { scaleY: 0.8 }],
    },
});

export default GlobalSettingsItem;
