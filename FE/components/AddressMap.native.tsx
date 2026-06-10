import { mapDarkStyle, mapStandardStyle } from '@/data/mapData';
import React from 'react';
import Text from '@/components/LocalizedText';
import { StyleSheet, View } from 'react-native';
import MapView, { Callout, Marker } from 'react-native-maps';
import { COLORS, FONTS, icons } from '../constants';

type AddressMapProps = {
    dark: boolean;
};

const AddressMap = ({ dark }: AddressMapProps) => {
    return (
        <MapView
            style={styles.map}
            customMapStyle={dark ? mapDarkStyle : mapStandardStyle}
            initialRegion={{
                latitude: 48.8566,
                longitude: 2.3522,
                latitudeDelta: 0.0922,
                longitudeDelta: 0.0421,
            }}>
            <Marker
                coordinate={{
                    latitude: 48.8566,
                    longitude: 2.3522,
                }}
                image={icons.location}
                title="Move"
                description="Address"
                onPress={() => console.log('Move to another screen')}>
                <Callout tooltip>
                    <View>
                        <View style={styles.bubble}>
                            <Text
                                style={{
                                    ...FONTS.body4,
                                    fontWeight: 'bold',
                                    color: COLORS.black,
                                }}>
                                User Address
                            </Text>
                        </View>
                        <View style={styles.arrowBorder} />
                        <View style={styles.arrow} />
                    </View>
                </Callout>
            </Marker>
        </MapView>
    );
};

const styles = StyleSheet.create({
    map: {
        height: '100%',
        zIndex: 1,
    },
    bubble: {
        flexDirection: 'column',
        alignSelf: 'flex-start',
        backgroundColor: '#fff',
        borderRadius: 6,
        borderColor: '#ccc',
        borderWidth: 0.5,
        padding: 15,
        width: 'auto',
    },
    arrow: {
        backgroundColor: 'transparent',
        borderColor: 'transparent',
        borderTopColor: '#fff',
        borderWidth: 16,
        alignSelf: 'center',
        marginTop: -32,
    },
    arrowBorder: {
        backgroundColor: 'transparent',
        borderColor: 'transparent',
        borderTopColor: '#007a87',
        borderWidth: 16,
        alignSelf: 'center',
        marginTop: -0.5,
    },
});

export default AddressMap;
