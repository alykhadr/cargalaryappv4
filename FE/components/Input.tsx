import React, { useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { View, StyleSheet, Image, TextInputProps } from 'react-native';
import { COLORS, SIZES } from '../constants';
import { useDirection } from '../theme/DirectionProvider';
import { useTheme } from '../theme/ThemeProvider';

interface InputProps extends TextInputProps {
    id: string;
    icon?: any;
    placeholderTextColor?: string;
    errorText?: string[];
    onInputChanged: (id: string, text: string) => void;
}

const Input: React.FC<InputProps> = (props) => {
    const [isFocused, setIsFocused] = useState(false);
    const { dark } = useTheme();
    const { textDirectionStyle } = useDirection();

    const handleFocus = () => {
        setIsFocused(true);
    };

    const handleBlur = () => {
        setIsFocused(false);
    };

    const onChangeText = (text: string) => {
        props.onInputChanged(props.id, text);
    };

    return (
        <View style={[styles.container]}>
            <View
                style={[
                    styles.inputContainer,
                    {
                        borderColor: isFocused
                            ? dark ? COLORS.primary100 : COLORS.primary
                            : dark ? COLORS.dark2 : COLORS.greyscale500,
                        backgroundColor: isFocused
                            ? COLORS.tansparentPrimary
                            : dark ? COLORS.dark2 : COLORS.greyscale500,
                    },
                ]}
            >
                {props.icon && (
                    <Image
                        source={props.icon}
                        style={[
                            styles.icon,
                            {
                                tintColor: isFocused
                                    ? dark ? COLORS.white : COLORS.primary
                                    : '#BCBCBC',
                            },
                        ]}
                    />
                )}
                <TextInput
                    {...props}
                    onChangeText={onChangeText}
                    onFocus={handleFocus}
                    onBlur={handleBlur}
                    style={[styles.input, textDirectionStyle, { color: dark ? COLORS.white : COLORS.black }]}
                    placeholder={props.placeholder}
                    placeholderTextColor={props.placeholderTextColor}
                    autoCapitalize="none"
                />
            </View>
            {props.errorText && (
                <View style={styles.errorContainer}>
                    <Text style={styles.errorText}>{props.errorText}</Text>
                </View>
            )}
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        width: '100%',
    },
    inputContainer: {
        width: '100%',
        paddingHorizontal: SIZES.padding,
        paddingVertical: SIZES.padding2,
        borderRadius: 12,
        borderWidth: 1,
        marginVertical: 5,
        flexDirection: 'row',
        height: 52,
        alignItems: 'center',
    },
    icon: {
        marginEnd: 10,
        height: 20,
        width: 20,
        tintColor: '#BCBCBC',
    },
    input: {
        color: COLORS.black,
        flex: 1,
        fontFamily: 'regular',
        fontSize: 14,
        paddingTop: 0,
    },
    errorContainer: {
        marginVertical: 4,
    },
    errorText: {
        color: 'red',
        fontSize: 12,
    },
});

export default Input;
