import React from 'react';
import { TextInput as RNTextInput, TextInputProps } from 'react-native';
import { useTranslation } from '@/hooks/useTranslation';

const LocalizedTextInput = React.forwardRef<RNTextInput, TextInputProps>(
    ({ placeholder, ...props }, ref) => {
        const { t } = useTranslation();

        return (
            <RNTextInput
                {...props}
                ref={ref}
                placeholder={placeholder ? t(placeholder) : placeholder}
            />
        );
    }
);

LocalizedTextInput.displayName = 'LocalizedTextInput';

export default LocalizedTextInput;
