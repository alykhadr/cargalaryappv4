import { ActivityIndicator, StyleSheet, View } from 'react-native';
import React, { useEffect, useMemo, useState } from 'react';
import Text from '@/components/LocalizedText';
import { SafeAreaView } from 'react-native-safe-area-context';
import { COLORS } from '../constants';
import Header from '../components/Header';
import { ScrollView } from 'react-native-virtualized-view';
import { useTheme } from '../theme/ThemeProvider';
import { WebView } from 'react-native-webview';
import { api } from '@/services/api';
import { PrivacyPolicy } from '@/types';
import { useTranslation } from '@/hooks/useTranslation';
import { useDirection } from '@/theme/DirectionProvider';

const SettingsPrivacyPolicy = () => {
    const { colors, dark } = useTheme();
    const { locale } = useTranslation();
    const { isRTL } = useDirection();
    const [privacyPolicy, setPrivacyPolicy] = useState<PrivacyPolicy | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [contentHeight, setContentHeight] = useState(1);

    useEffect(() => {
        let isMounted = true;

        const loadPrivacyPolicy = async () => {
            try {
                const response = await api.getPrivacyPolicy();
                if (isMounted) {
                    setPrivacyPolicy(response);
                    setError(null);
                }
            } catch (err) {
                if (isMounted) {
                    setError(err instanceof Error ? err.message : 'Unable to load privacy policy.');
                }
            } finally {
                if (isMounted) {
                    setIsLoading(false);
                }
            }
        };

        loadPrivacyPolicy();

        return () => {
            isMounted = false;
        };
    }, []);

    const content = useMemo(() => {
        if (!privacyPolicy) {
            return '';
        }

        return locale === 'ar'
            ? privacyPolicy.privacyPolicyAr || privacyPolicy.privacyPolicyEn || ''
            : privacyPolicy.privacyPolicyEn || privacyPolicy.privacyPolicyAr || '';
    }, [locale, privacyPolicy]);

    const html = useMemo(() => buildHtml(content, dark, isRTL), [content, dark, isRTL]);

    return (
        <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
            <View style={[styles.container, { backgroundColor: colors.background }]}>
                <Header title="Privacy Policy" />
                <ScrollView showsVerticalScrollIndicator={false}>
                    {isLoading ? (
                        <View style={styles.stateContainer}>
                            <ActivityIndicator size="large" color={COLORS.primary} />
                        </View>
                    ) : error ? (
                        <View style={styles.stateContainer}>
                            <Text style={[styles.message, { color: dark ? COLORS.secondaryWhite : COLORS.greyscale900 }]}>
                                {error}
                            </Text>
                        </View>
                    ) : content ? (
                        <WebView
                            originWhitelist={['*']}
                            source={{ html }}
                            style={[styles.webView, { height: contentHeight }]}
                            scrollEnabled={false}
                            showsVerticalScrollIndicator={false}
                            onMessage={(event) => {
                                const nextHeight = Number(event.nativeEvent.data);
                                if (Number.isFinite(nextHeight) && nextHeight > 0) {
                                    setContentHeight(nextHeight);
                                }
                            }}
                            injectedJavaScript={heightScript}
                        />
                    ) : (
                        <View style={styles.stateContainer}>
                            <Text style={[styles.message, { color: dark ? COLORS.secondaryWhite : COLORS.greyscale900 }]}>
                                Privacy policy is not available.
                            </Text>
                        </View>
                    )}
                </ScrollView>
            </View>
        </SafeAreaView>
    );
};

const heightScript = `
  setTimeout(function() {
    var height = Math.max(
      document.body.scrollHeight,
      document.documentElement.scrollHeight,
      document.body.offsetHeight,
      document.documentElement.offsetHeight
    );
    window.ReactNativeWebView.postMessage(String(height));
  }, 100);
  true;
`;

const buildHtml = (content: string, dark: boolean, isRTL: boolean) => {
    const textColor = dark ? '#F6F7FB' : '#212529';
    const mutedColor = dark ? '#CED4DA' : '#495057';
    const backgroundColor = dark ? '#000000' : '#FFFFFF';
    const direction = isRTL ? 'rtl' : 'ltr';
    const align = isRTL ? 'right' : 'left';

    return `<!doctype html>
<html dir="${direction}">
  <head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <style>
      html, body {
        margin: 0;
        padding: 0;
        background: ${backgroundColor};
        color: ${textColor};
        direction: ${direction};
        text-align: ${align};
        font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Arial, sans-serif;
        font-size: 16px;
        line-height: 1.7;
      }
      body {
        overflow: hidden;
      }
      h1, h2, h3, h4, h5, h6 {
        color: ${textColor};
        line-height: 1.35;
        margin: 22px 0 10px;
      }
      p {
        margin: 0 0 14px;
        color: ${mutedColor};
      }
      ul, ol {
        margin: 0 0 16px;
        padding-inline-start: 24px;
      }
      li {
        margin-bottom: 8px;
      }
      a {
        color: #405FF2;
      }
      img, table {
        max-width: 100%;
      }
      blockquote {
        border-inline-start: 4px solid #405FF2;
        margin: 16px 0;
        padding: 8px 14px;
        color: ${mutedColor};
      }
    </style>
  </head>
  <body>${content}</body>
</html>`;
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
    stateContainer: {
        minHeight: 220,
        alignItems: 'center',
        justifyContent: 'center'
    },
    message: {
        fontSize: 16,
        fontFamily: 'regular',
        color: COLORS.black,
        textAlign: 'center'
    },
    webView: {
        width: '100%',
        backgroundColor: 'transparent'
    }
});

export default SettingsPrivacyPolicy;
