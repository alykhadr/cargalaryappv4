import { useDirection } from '@/theme/DirectionProvider';
import { getLocaleFromLanguage, translateText } from '@/utils/translations';

export const useTranslation = () => {
    const { language } = useDirection();
    const locale = getLocaleFromLanguage(language);

    return {
        locale,
        t: (value: string) => translateText(value, locale),
    };
};
