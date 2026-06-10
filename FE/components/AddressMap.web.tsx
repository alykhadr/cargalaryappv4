import React from 'react';
import Text from '@/components/LocalizedText';
import { StyleSheet, View } from 'react-native';
import { COLORS, FONTS } from '../constants';

type AddressMapProps = {
    dark: boolean;
};

const AddressMap = ({ dark }: AddressMapProps) => {
    return (
        <View style={[styles.mapFallback, { backgroundColor: dark ? COLORS.dark2 : COLORS.grayscale100 }]}>
            <View style={styles.marker}>
                <Text style={{ ...FONTS.body4, color: COLORS.white }}>User Address</Text>
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    mapFallback: {
        height: '100%',
        zIndex: 1,
        alignItems: 'center',
        justifyContent: 'center',
    },
    marker: {
        backgroundColor: COLORS.primary,
        borderRadius: 8,
        paddingHorizontal: 16,
        paddingVertical: 10,
    },
});

export default AddressMap;
