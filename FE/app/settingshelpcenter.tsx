import { View, StyleSheet, TouchableOpacity, Image, LayoutAnimation, Linking, Alert } from 'react-native';
import React, { useEffect, useMemo, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { COLORS, SIZES, icons } from '../constants';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTheme } from '../theme/ThemeProvider';
import { ScrollView } from 'react-native-virtualized-view';
import { NavigationProp, useNavigation } from '@react-navigation/native';
import HelpCenterItem from '@/components/HelpCenterItem';
import { api } from '../services/api';
import { CompanyInformation, FAQItem } from '../types';
import { useDirection } from '../theme/DirectionProvider';

type HelpTab = 'faq' | 'contact';

const HelpCenter = () => {
  const navigation = useNavigation<NavigationProp<any>>();
  const { dark, colors } = useTheme();
  const { language } = useDirection();
  const [activeTab, setActiveTab] = useState<HelpTab>('faq');
  const [searchText, setSearchText] = useState('');
  const [expandedId, setExpandedId] = useState<number | null>(null);
  const [faqs, setFaqs] = useState<FAQItem[]>([]);
  const [companyInfo, setCompanyInfo] = useState<CompanyInformation | null>(null);
  const [loadingFaqs, setLoadingFaqs] = useState(true);

  useEffect(() => {
    api.getFAQs()
      .then((items) => setFaqs(items.filter((item) => item.isAvailable).sort((a, b) => a.order - b.order)))
      .catch(() => setFaqs([]))
      .finally(() => setLoadingFaqs(false));

    api.getCompanyInformation()
      .then(setCompanyInfo)
      .catch(() => setCompanyInfo(null));
  }, []);

  const filteredFaqs = useMemo(() => {
    const query = searchText.trim().toLowerCase();
    return faqs.filter((item) => {
      const title = language === 'Arabic'
        ? item.titleAr || item.titleEn || ''
        : item.titleEn || item.titleAr || '';
      const description = language === 'Arabic'
        ? item.descriptionAr || item.descriptionEn || ''
        : item.descriptionEn || item.descriptionAr || '';

      if (!query) {
        return true;
      }

      return title.toLowerCase().includes(query) || description.toLowerCase().includes(query);
    });
  }, [faqs, language, searchText]);

  const openUrl = async (url: string, fallbackMessage: string) => {
    try {
      const supported = await Linking.canOpenURL(url);
      if (!supported) {
        Alert.alert('Unavailable', fallbackMessage);
        return;
      }

      await Linking.openURL(url);
    } catch {
      Alert.alert('Unavailable', fallbackMessage);
    }
  };

  const toggleExpand = (id: number) => {
    LayoutAnimation.configureNext(LayoutAnimation.Presets.easeInEaseOut);
    setExpandedId((current) => current === id ? null : id);
  };

  const mobileNumber = companyInfo?.mobileNo?.trim() || companyInfo?.telNo?.trim() || '';
  const cleanPhone = mobileNumber.replace(/[^\d+]/g, '');
  const email = companyInfo?.email?.trim() || '';

  const renderHeader = () => (
    <View style={styles.headerContainer}>
      <View style={styles.headerLeft}>
        <TouchableOpacity onPress={() => navigation.goBack()}>
          <Image
            source={icons.back}
            resizeMode="contain"
            style={[styles.backIcon, { tintColor: dark ? COLORS.white : COLORS.greyscale900 }]}
          />
        </TouchableOpacity>
        <Text style={[styles.headerTitle, { color: dark ? COLORS.white : COLORS.greyscale900 }]}>
          Help Center
        </Text>
      </View>
    </View>
  );

  const renderTabs = () => (
    <View
      style={[
        styles.tabRow,
        { backgroundColor: dark ? COLORS.dark2 : COLORS.grayscale100 },
      ]}
    >
      {[
        { key: 'faq' as const, label: 'FAQ' },
        { key: 'contact' as const, label: 'Contact Us' },
      ].map((item) => {
        const active = activeTab === item.key;
        return (
          <TouchableOpacity
            key={item.key}
            activeOpacity={0.85}
            onPress={() => setActiveTab(item.key)}
            style={[
              styles.tabButton,
              active && { backgroundColor: dark ? COLORS.white : COLORS.primary },
            ]}
          >
            <Text
              style={[
                styles.tabButtonText,
                { color: active ? (dark ? COLORS.black : COLORS.white) : (dark ? COLORS.white : COLORS.primary) },
              ]}
            >
              {item.label}
            </Text>
          </TouchableOpacity>
        );
      })}
    </View>
  );

  const renderFaqTab = () => (
    <View>
      <View
        style={[
          styles.searchBar,
          { backgroundColor: dark ? COLORS.dark2 : COLORS.grayscale100 },
        ]}
      >
        <Image
          source={icons.search}
          resizeMode="contain"
          style={[styles.searchIcon, { tintColor: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}
        />
        <TextInput
          style={[styles.input, { color: dark ? COLORS.white : COLORS.greyscale900 }]}
          placeholder="Search FAQ"
          placeholderTextColor={dark ? COLORS.grayscale400 : COLORS.grayscale700}
          value={searchText}
          onChangeText={setSearchText}
        />
      </View>

      <View style={styles.contentBlock}>
        {loadingFaqs ? (
          <Text style={[styles.stateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            Loading FAQs...
          </Text>
        ) : filteredFaqs.length === 0 ? (
          <Text style={[styles.stateText, { color: dark ? COLORS.grayscale400 : COLORS.grayscale700 }]}>
            No FAQ items matched your search.
          </Text>
        ) : (
          filteredFaqs.map((faq) => {
            const title = language === 'Arabic'
              ? faq.titleAr || faq.titleEn || 'FAQ'
              : faq.titleEn || faq.titleAr || 'FAQ';
            const description = language === 'Arabic'
              ? stripHtml(faq.descriptionAr || faq.descriptionEn || '')
              : stripHtml(faq.descriptionEn || faq.descriptionAr || '');
            const expanded = expandedId === faq.id;

            return (
              <View
                key={faq.id}
                style={[
                  styles.faqContainer,
                  { backgroundColor: dark ? COLORS.dark2 : COLORS.white },
                ]}
              >
                <TouchableOpacity onPress={() => toggleExpand(faq.id)} activeOpacity={0.85}>
                  <View style={styles.questionContainer}>
                    <Text style={[styles.question, { color: dark ? COLORS.white : COLORS.black }]}>
                      {title}
                    </Text>
                    <Text style={[styles.icon, { color: dark ? COLORS.white : COLORS.black }]}>
                      {expanded ? '-' : '+'}
                    </Text>
                  </View>
                </TouchableOpacity>
                {expanded ? (
                  <Text style={[styles.answer, { color: dark ? COLORS.grayscale200 : COLORS.gray2 }]}>
                    {description}
                  </Text>
                ) : null}
              </View>
            );
          })
        )}
      </View>
    </View>
  );

  const renderContactTab = () => (
    <View style={styles.contentBlock}>
      <HelpCenterItem
        icon={icons.headset}
        title={cleanPhone ? `Customer Service: ${cleanPhone}` : 'Customer Service not configured'}
        onPress={() => cleanPhone
          ? openUrl(`tel:${cleanPhone}`, 'Unable to call customer service right now.')
          : Alert.alert('Unavailable', 'No customer service number is configured yet.')}
      />
      <HelpCenterItem
        icon={icons.whatsapp}
        title={cleanPhone ? `WhatsApp: ${cleanPhone}` : 'WhatsApp not configured'}
        onPress={() => cleanPhone
          ? openUrl(`https://wa.me/${cleanPhone.replace(/[^\d]/g, '')}`, 'Unable to open WhatsApp right now.')
          : Alert.alert('Unavailable', 'No WhatsApp number is configured yet.')}
      />
      <HelpCenterItem
        icon={icons.email2}
        title={email ? `Email: ${email}` : 'Email not configured'}
        onPress={() => email
          ? openUrl(`mailto:${email}`, 'Unable to open your email app right now.')
          : Alert.alert('Unavailable', 'No support email is configured yet.')}
      />

      {companyInfo?.aboutUsEn || companyInfo?.aboutUsAr ? (
        <View
          style={[
            styles.aboutCard,
            { backgroundColor: dark ? COLORS.dark2 : COLORS.white },
          ]}
        >
          <Text style={[styles.aboutTitle, { color: dark ? COLORS.white : COLORS.black }]}>
            About Us
          </Text>
          <Text style={[styles.aboutText, { color: dark ? COLORS.grayscale200 : COLORS.grayscale700 }]}>
            {language === 'Arabic'
              ? companyInfo?.aboutUsAr || companyInfo?.aboutUsEn
              : companyInfo?.aboutUsEn || companyInfo?.aboutUsAr}
          </Text>
        </View>
      ) : null}
    </View>
  );

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: colors.background }]}>
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        {renderHeader()}
        {renderTabs()}
        <ScrollView showsVerticalScrollIndicator={false}>
          {activeTab === 'faq' ? renderFaqTab() : renderContactTab()}
        </ScrollView>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  area: {
    flex: 1,
    backgroundColor: COLORS.white,
  },
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
    padding: 16,
  },
  headerContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  headerLeft: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  backIcon: {
    height: 24,
    width: 24,
    marginRight: 16,
  },
  headerTitle: {
    fontSize: 24,
    fontFamily: 'bold',
  },
  tabRow: {
    marginTop: 18,
    marginBottom: 18,
    padding: 4,
    borderRadius: 18,
    flexDirection: 'row',
  },
  tabButton: {
    flex: 1,
    minHeight: 44,
    borderRadius: 14,
    alignItems: 'center',
    justifyContent: 'center',
  },
  tabButtonText: {
    fontSize: 15,
    fontFamily: 'bold',
  },
  searchBar: {
    width: SIZES.width - 32,
    height: 56,
    borderRadius: 16,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 16,
  },
  searchIcon: {
    width: 24,
    height: 24,
  },
  input: {
    flex: 1,
    marginHorizontal: 12,
    fontFamily: 'regular',
  },
  contentBlock: {
    marginTop: 18,
    paddingBottom: 24,
  },
  faqContainer: {
    marginBottom: 16,
    borderRadius: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.08,
    shadowRadius: 4,
    elevation: 1,
  },
  questionContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 16,
  },
  question: {
    flex: 1,
    fontSize: 16,
    fontFamily: 'semiBold',
    paddingRight: 16,
  },
  icon: {
    fontSize: 20,
    fontFamily: 'bold',
  },
  answer: {
    fontSize: 14,
    paddingHorizontal: 16,
    paddingBottom: 16,
    lineHeight: 21,
    fontFamily: 'regular',
  },
  aboutCard: {
    marginTop: 6,
    borderRadius: 16,
    padding: 16,
  },
  aboutTitle: {
    fontSize: 16,
    fontFamily: 'bold',
    marginBottom: 8,
  },
  aboutText: {
    fontSize: 14,
    fontFamily: 'regular',
    lineHeight: 21,
  },
  stateText: {
    fontSize: 15,
    fontFamily: 'medium',
    textAlign: 'center',
    marginTop: 24,
    lineHeight: 22,
  },
});

export default HelpCenter;

function stripHtml(value: string) {
  return value
    .replace(/<[^>]*>/g, ' ')
    .replace(/&nbsp;/gi, ' ')
    .replace(/\s+/g, ' ')
    .trim();
}
