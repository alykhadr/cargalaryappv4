import { View, StyleSheet } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import { COLORS } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import Header from '../components/Header';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import LanguageItem from '@/components/LanguageItem';
import { LANGUAGE_OPTIONS, useDirection } from '@/theme/DirectionProvider';

const SettingsLanguage = () => {
    const { colors, dark } = useTheme();
    const { language, setLanguage, textDirectionStyle } = useDirection();

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <Header title="Language & Region" />
                <ScrollView showsVerticalScrollIndicator={false}>
                    <Text style={[styles.title, textDirectionStyle, { color: dark ? COLORS.white : COLORS.black }]}>
                        Select Language
                    </Text>
                    <View style={{ marginTop: 12 }}>
                        {LANGUAGE_OPTIONS.map((item) => (
                            <LanguageItem
                                checked={language === item.name}
                                direction={item.direction}
                                key={item.name}
                                name={item.name}
                                onPress={() => setLanguage(item.name)}
                            />
                        ))}
                    </View>
                </ScrollView>
            </View>
        </SafeAreaView>
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
    title: {
        fontSize: 20,
        fontFamily: "bold",
        color: COLORS.black,
        marginVertical: 16
    }
});

export default SettingsLanguage;
