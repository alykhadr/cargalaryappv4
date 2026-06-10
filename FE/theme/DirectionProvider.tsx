import AsyncStorage from '@react-native-async-storage/async-storage';
import React, {
    ReactNode,
    createContext,
    useCallback,
    useContext,
    useEffect,
    useMemo,
    useState,
} from 'react';
import { I18nManager, ImageStyle, Platform, TextStyle, ViewStyle } from 'react-native';

export type AppDirection = 'ltr' | 'rtl';

export type LanguageOption = {
    name: string;
    direction: AppDirection;
    suggested?: boolean;
};

export const LANGUAGE_OPTIONS: LanguageOption[] = [
    { name: 'English', direction: 'ltr' },
    { name: 'Arabic', direction: 'rtl' },
];

const DIRECTION_STORAGE_KEY = '@carea:language-direction';
const DEFAULT_LANGUAGE = LANGUAGE_OPTIONS[0].name;
const DEFAULT_DIRECTION = LANGUAGE_OPTIONS[0].direction;

type DirectionContextType = {
    direction: AppDirection;
    isRTL: boolean;
    language: string;
    isReady: boolean;
    setDirection: (direction: AppDirection) => Promise<void>;
    setLanguage: (language: string) => Promise<void>;
    rootDirectionStyle: ViewStyle;
    textDirectionStyle: TextStyle;
    iconFlipStyle: ImageStyle;
};

const defaultDirectionContext: DirectionContextType = {
    direction: DEFAULT_DIRECTION,
    isRTL: false,
    language: DEFAULT_LANGUAGE,
    isReady: false,
    setDirection: async () => {},
    setLanguage: async () => {},
    rootDirectionStyle: { direction: DEFAULT_DIRECTION },
    textDirectionStyle: {
        writingDirection: DEFAULT_DIRECTION,
        textAlign: 'left',
    },
    iconFlipStyle: {
        transform: [{ scaleX: 1 }],
    },
};

export const DirectionContext = createContext<DirectionContextType>(defaultDirectionContext);

type DirectionProviderProps = {
    children: ReactNode;
};

const getDirectionForLanguage = (language: string): AppDirection => {
    return LANGUAGE_OPTIONS.find((item) => item.name === language)?.direction ?? DEFAULT_DIRECTION;
};

const normalizeDirection = (value: unknown): AppDirection | null => {
    return value === 'rtl' || value === 'ltr' ? value : null;
};

type DirectionAwareI18nManager = typeof I18nManager & {
    swapLeftAndRightInRTL?: (flipStyles: boolean) => void;
};

const applyNativeDirection = (direction: AppDirection) => {
    const isRTL = direction === 'rtl';
    const i18nManager = I18nManager as DirectionAwareI18nManager;

    if (Platform.OS === 'web' && typeof document !== 'undefined') {
        document.documentElement.setAttribute('dir', direction);
        document.documentElement.style.direction = direction;
        return;
    }

    if (typeof i18nManager.allowRTL === 'function') {
        i18nManager.allowRTL(true);
    }

    if (typeof i18nManager.forceRTL === 'function') {
        i18nManager.forceRTL(isRTL);
    }

    if (typeof i18nManager.swapLeftAndRightInRTL === 'function') {
        i18nManager.swapLeftAndRightInRTL(isRTL);
    }
};

const persistDirectionPreference = async (language: string, direction: AppDirection) => {
    await AsyncStorage.setItem(
        DIRECTION_STORAGE_KEY,
        JSON.stringify({ language, direction })
    );
};

export const DirectionProvider: React.FC<DirectionProviderProps> = ({ children }) => {
    const [language, setCurrentLanguage] = useState(DEFAULT_LANGUAGE);
    const [direction, setCurrentDirection] = useState<AppDirection>(DEFAULT_DIRECTION);
    const [isReady, setIsReady] = useState(false);

    useEffect(() => {
        let isMounted = true;

        const loadStoredDirection = async () => {
            try {
                const storedPreference = await AsyncStorage.getItem(DIRECTION_STORAGE_KEY);

                if (storedPreference) {
                    const parsedPreference = JSON.parse(storedPreference);
                    const nextLanguage =
                        typeof parsedPreference.language === 'string'
                            ? parsedPreference.language
                            : DEFAULT_LANGUAGE;
                    const nextDirection =
                        normalizeDirection(parsedPreference.direction) ??
                        getDirectionForLanguage(nextLanguage);

                    if (isMounted) {
                        setCurrentLanguage(nextLanguage);
                        setCurrentDirection(nextDirection);
                    }

                    applyNativeDirection(nextDirection);
                } else {
                    applyNativeDirection(DEFAULT_DIRECTION);
                }
            } catch {
                applyNativeDirection(DEFAULT_DIRECTION);
            } finally {
                if (isMounted) {
                    setIsReady(true);
                }
            }
        };

        loadStoredDirection();

        return () => {
            isMounted = false;
        };
    }, []);

    const setLanguage = useCallback(async (nextLanguage: string) => {
        const nextDirection = getDirectionForLanguage(nextLanguage);

        setCurrentLanguage(nextLanguage);
        setCurrentDirection(nextDirection);
        applyNativeDirection(nextDirection);
        await persistDirectionPreference(nextLanguage, nextDirection);
    }, []);

    const setDirection = useCallback(
        async (nextDirection: AppDirection) => {
            setCurrentDirection(nextDirection);
            applyNativeDirection(nextDirection);
            await persistDirectionPreference(language, nextDirection);
        },
        [language]
    );

    const isRTL = direction === 'rtl';

    const rootDirectionStyle = useMemo<ViewStyle>(
        () => ({ direction }),
        [direction]
    );

    const textDirectionStyle = useMemo<TextStyle>(
        () => ({
            writingDirection: direction,
            textAlign: isRTL ? 'right' : 'left',
        }),
        [direction, isRTL]
    );

    const iconFlipStyle = useMemo<ImageStyle>(
        () => ({
            transform: [{ scaleX: isRTL ? -1 : 1 }],
        }),
        [isRTL]
    );

    const value = useMemo(
        () => ({
            direction,
            isRTL,
            language,
            isReady,
            setDirection,
            setLanguage,
            rootDirectionStyle,
            textDirectionStyle,
            iconFlipStyle,
        }),
        [
            direction,
            iconFlipStyle,
            isRTL,
            isReady,
            language,
            rootDirectionStyle,
            setDirection,
            setLanguage,
            textDirectionStyle,
        ]
    );

    return (
        <DirectionContext.Provider value={value}>
            {children}
        </DirectionContext.Provider>
    );
};

export const useDirection = () => useContext(DirectionContext);
