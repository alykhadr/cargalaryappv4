import { MaterialCommunityIcons } from '@expo/vector-icons';
import { useNavigation } from 'expo-router';
import React, { useEffect, useMemo, useState } from 'react';
import Text from '@/components/LocalizedText';
import TextInput from '@/components/LocalizedTextInput';
import { Alert, Image, Modal, ScrollView, StyleSheet, TouchableOpacity, TouchableWithoutFeedback, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import ButtonFilled from '../components/ButtonFilled';
import Header from '../components/Header';
import Input from '../components/Input';
import { COLORS, icons, SIZES } from '../constants';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../theme/ThemeProvider';
import { launchImagePicker } from '../utils/ImagePickerHelper';

type Nav = {
  goBack: () => void;
};

type AreaItem = {
  code: string;
  item: string;
  callingCode: string;
  flag: string;
};

const EditProfile = () => {
  const navigation = useNavigation<Nav>();
  const { dark } = useTheme();
  const { user, updateProfile } = useAuth();

  const [image, setImage] = useState<{ uri: string } | null>(null);
  const [areas, setAreas] = useState<AreaItem[]>([]);
  const [selectedArea, setSelectedArea] = useState<AreaItem | null>(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  const [fullName, setFullName] = useState(user?.fullName ?? '');
  const [phoneNumber, setPhoneNumber] = useState(user?.phoneNumber ?? '');

  useEffect(() => {
    setFullName(user?.fullName ?? '');
    setPhoneNumber(user?.phoneNumber ?? '');
  }, [user]);

  useEffect(() => {
    let isMounted = true;

    fetch('https://restcountries.com/v3.1/all?fields=cca2,name,idd,flags')
      .then((response) => response.json())
      .then((data) => {
        if (!isMounted) {
          return;
        }

        const areaData: AreaItem[] = data
          .filter(
            (item: any) =>
              item.cca2 &&
              item.name?.common &&
              item.idd?.root &&
              Array.isArray(item.idd.suffixes) &&
              item.idd.suffixes.length > 0
          )
          .map((item: any) => ({
            code: item.cca2,
            item: item.name.common,
            callingCode: `${item.idd.root}${item.idd.suffixes[0]}`,
            flag: `https://flagsapi.com/${item.cca2}/flat/64.png`,
          }))
          .sort((a: AreaItem, b: AreaItem) => a.item.localeCompare(b.item));

        setAreas(areaData);

        const preferredCode =
          areaData.find((entry) => entry.item === user?.country)?.code ||
          areaData.find((entry) => entry.code === 'SA')?.code ||
          areaData.find((entry) => entry.code === 'US')?.code;

        if (preferredCode) {
          setSelectedArea(areaData.find((entry) => entry.code === preferredCode) ?? null);
        }
      })
      .catch(() => {})
      .finally(() => {
        if (isMounted && user?.country && !selectedArea) {
          setSelectedArea({
            code: '',
            item: user.country,
            callingCode: '',
            flag: '',
          });
        }
      });

    return () => {
      isMounted = false;
    };
  }, [user?.country]);

  const effectiveCountry = useMemo(
    () => selectedArea?.item || user?.country || '',
    [selectedArea, user?.country]
  );

  const pickImage = async () => {
    try {
      const tempUri = await launchImagePicker();
      if (!tempUri) {
        return;
      }
      setImage({ uri: tempUri });
    } catch {}
  };

  const handleUpdate = async () => {
    if (!fullName.trim()) {
      Alert.alert('Error', 'Full name is required');
      return;
    }

    setIsSaving(true);
    try {
      await updateProfile({
        fullName: fullName.trim(),
        phoneNumber: phoneNumber.trim() || undefined,
        country: effectiveCountry || undefined,
        avatarUrl: user?.avatarUrl,
      });
      Alert.alert('Profile Updated', 'Your profile information has been saved.');
      navigation.goBack();
    } catch (error: any) {
      Alert.alert('Update Failed', error?.message || 'Unable to update your profile');
    } finally {
      setIsSaving(false);
    }
  };

  const renderAreasCodesModal = () => (
    <Modal animationType="slide" transparent visible={modalVisible}>
      <TouchableWithoutFeedback onPress={() => setModalVisible(false)}>
        <View style={styles.modalOverlay}>
          <View style={styles.modalCard}>
            <ScrollView contentContainerStyle={styles.modalContent}>
              {areas.map((item) => (
                <TouchableOpacity
                  key={item.code}
                  style={styles.areaRow}
                  onPress={() => {
                    setSelectedArea(item);
                    setModalVisible(false);
                  }}
                >
                  <Image source={{ uri: item.flag }} resizeMode="contain" style={styles.areaFlag} />
                  <Text style={styles.areaText}>{item.item}</Text>
                </TouchableOpacity>
              ))}
            </ScrollView>
          </View>
        </View>
      </TouchableWithoutFeedback>
    </Modal>
  );

  return (
    <SafeAreaView style={[styles.area, { backgroundColor: dark ? COLORS.dark1 : COLORS.white }]}>
      <View style={[styles.container, { backgroundColor: dark ? COLORS.dark1 : COLORS.white }]}>
        <Header title="Edit Profile" />
        <ScrollView showsVerticalScrollIndicator={false}>
          <View style={styles.avatarSection}>
            <View style={styles.avatarContainer}>
              <Image
                source={image ?? (user?.avatarUrl ? { uri: user.avatarUrl } : icons.userDefault2)}
                resizeMode="cover"
                style={styles.avatar}
              />
              <TouchableOpacity onPress={pickImage} style={styles.pickImage}>
                <MaterialCommunityIcons name="pencil-outline" size={24} color={COLORS.white} />
              </TouchableOpacity>
            </View>
            <Text style={[styles.helperText, { color: dark ? COLORS.grayscale400 : COLORS.greyscale600 }]}>
              Avatar preview updates locally. Profile details save to the API.
            </Text>
          </View>

          <Input
            id="fullName"
            value={fullName}
            onInputChanged={(_, value) => setFullName(value)}
            placeholder="Full Name"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
          />
          <Input
            id="email"
            value={user?.email ?? ''}
            onInputChanged={() => {}}
            placeholder="Email"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
            editable={false}
          />

          <View
            style={[
              styles.inputContainer,
              {
                backgroundColor: dark ? COLORS.dark2 : COLORS.greyscale500,
                borderColor: dark ? COLORS.dark2 : COLORS.greyscale500,
              },
            ]}
          >
            <TouchableOpacity style={styles.selectFlagContainer} onPress={() => setModalVisible(true)}>
              <View style={{ justifyContent: 'center' }}>
                <Image source={icons.down} resizeMode="contain" style={styles.downIcon} />
              </View>
              {selectedArea?.flag ? (
                <View style={{ justifyContent: 'center', marginLeft: 5 }}>
                  <Image source={{ uri: selectedArea.flag }} resizeMode="contain" style={styles.flagIcon} />
                </View>
              ) : null}
              <View style={{ justifyContent: 'center', marginLeft: 5 }}>
                <Text style={{ color: dark ? COLORS.white : '#111', fontSize: 12 }}>
                  {selectedArea?.callingCode || effectiveCountry || 'Country'}
                </Text>
              </View>
            </TouchableOpacity>
            <TextInput
              style={[styles.input, { color: dark ? COLORS.white : COLORS.black }]}
              placeholder="Enter your phone number"
              placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
              selectionColor="#111"
              keyboardType="phone-pad"
              value={phoneNumber}
              onChangeText={setPhoneNumber}
            />
          </View>

          <Input
            id="country"
            value={effectiveCountry}
            onInputChanged={(_, value) => setSelectedArea({ code: '', item: value, callingCode: '', flag: '' })}
            placeholder="Country"
            placeholderTextColor={dark ? COLORS.grayTie : COLORS.black}
          />
        </ScrollView>
      </View>

      {renderAreasCodesModal()}
      <View style={styles.bottomContainer}>
        <ButtonFilled
          title={isSaving ? 'Updating...' : 'Update'}
          style={styles.continueButton}
          onPress={handleUpdate}
        />
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
    padding: 16,
    backgroundColor: COLORS.white,
  },
  avatarSection: {
    alignItems: 'center',
    marginVertical: 12,
  },
  avatarContainer: {
    alignItems: 'center',
    width: 130,
    height: 130,
    borderRadius: 65,
  },
  avatar: {
    height: 130,
    width: 130,
    borderRadius: 65,
  },
  helperText: {
    fontSize: 12,
    marginTop: 10,
    textAlign: 'center',
  },
  pickImage: {
    height: 42,
    width: 42,
    borderRadius: 21,
    backgroundColor: COLORS.primary,
    alignItems: 'center',
    justifyContent: 'center',
    position: 'absolute',
    bottom: 0,
    right: 0,
  },
  inputContainer: {
    flexDirection: 'row',
    borderColor: COLORS.greyscale500,
    borderWidth: 0.4,
    borderRadius: 6,
    height: 52,
    width: SIZES.width - 32,
    alignItems: 'center',
    marginVertical: 16,
    backgroundColor: COLORS.greyscale500,
  },
  downIcon: {
    width: 10,
    height: 10,
    tintColor: '#111',
  },
  selectFlagContainer: {
    width: 110,
    height: 50,
    marginHorizontal: 5,
    flexDirection: 'row',
  },
  flagIcon: {
    width: 30,
    height: 30,
  },
  input: {
    flex: 1,
    marginVertical: 10,
    height: 40,
    fontSize: 14,
    color: '#111',
  },
  bottomContainer: {
    position: 'absolute',
    bottom: 32,
    right: 16,
    left: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: SIZES.width - 32,
    alignItems: 'center',
  },
  continueButton: {
    width: SIZES.width - 32,
    borderRadius: 32,
    backgroundColor: COLORS.primary,
    borderColor: COLORS.primary,
  },
  modalOverlay: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(0,0,0,0.35)',
  },
  modalCard: {
    height: SIZES.height * 0.65,
    width: SIZES.width * 0.84,
    backgroundColor: COLORS.primary,
    borderRadius: 12,
  },
  modalContent: {
    padding: 20,
  },
  areaRow: {
    paddingVertical: 10,
    flexDirection: 'row',
    alignItems: 'center',
  },
  areaFlag: {
    height: 30,
    width: 30,
    marginRight: 10,
  },
  areaText: {
    fontSize: 16,
    color: '#fff',
  },
});

export default EditProfile;
