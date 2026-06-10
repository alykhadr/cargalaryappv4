import React from 'react';
import { StyleSheet, Text as RNText, TextProps, TextStyle } from 'react-native';
import { useTranslation } from '@/hooks/useTranslation';
import { useDirection } from '@/theme/DirectionProvider';

const LocalizedText: React.FC<TextProps> = ({ children, ...props }) => {
    const { t } = useTranslation();
    const { isRTL } = useDirection();
    const flattenedStyle = StyleSheet.flatten(props.style);
    const directionStyle: TextStyle = {
        writingDirection: isRTL ? 'rtl' : 'ltr',
        ...(!flattenedStyle?.textAlign ? { textAlign: isRTL ? 'right' : 'left' } : {}),
    };

    const localizeChild = (child: React.ReactNode): React.ReactNode => {
        if (typeof child === 'string') {
            return t(child);
        }

        if (Array.isArray(child)) {
            return child.map((item, index) => (
                <React.Fragment key={index}>{localizeChild(item)}</React.Fragment>
            ));
        }

        return child;
    };

    return (
        <RNText {...props} style={[directionStyle, props.style]}>
            {localizeChild(children)}
        </RNText>
    );
};

export default LocalizedText;
