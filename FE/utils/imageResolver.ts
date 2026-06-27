import { icons, images } from '../constants';
import { Platform } from 'react-native';

const API_ORIGIN = (
  process.env.EXPO_PUBLIC_CARGALARY_ASSET_URL ||
  (Platform.OS === 'android'
    ? 'http://10.0.2.2:5121'
    : 'http://localhost:5121')
).replace(/\/api\/?$/, '').replace(/\/$/, '');

const CAR_IMAGE_MAP: Record<string, any> = {
  // BMW
  bmw1: images.bmw1,
  bmw2: images.bmw2,
  bmw3: images.bmw3,
  bmw4: images.bmw4,
  bmw5: images.bmw5,
  bmw6: images.bmw6,
  bmw7: images.bmw7,
  bmw8: images.bmw8,
  bmw9: images.bmw9,
  bmw10: images.bmw10,
  bmw11: images.bmw11,
  bmw12: images.bmw12,
  // Bugatti
  bugatti1: images.bugatti1,
  bugatti2: images.bugatti2,
  bugatti3: images.bugatti3,
  bugatti4: images.bugatti4,
  bugatti5: images.bugatti5,
  bugatti6: images.bugatti6,
  bugatti7: images.bugatti7,
  bugatti8: images.bugatti8,
  bugatti9: images.bugatti9,
  bugatti10: images.bugatti10,
  bugatti11: images.bugatti11,
  bugatti12: images.bugatti12,
  // Honda
  honda1: images.honda1,
  honda2: images.honda2,
  honda3: images.honda3,
  honda4: images.honda4,
  honda5: images.honda5,
  honda6: images.honda6,
  honda7: images.honda7,
  honda8: images.honda8,
  honda9: images.honda9,
  honda10: images.honda10,
  honda11: images.honda11,
  honda12: images.honda12,
  // Mercedes
  mercedes1: images.mercedes1,
  mercedes2: images.mercedes2,
  mercedes3: images.mercedes3,
  mercedes4: images.mercedes4,
  mercedes5: images.mercedes5,
  mercedes6: images.mercedes6,
  mercedes7: images.mercedes7,
  mercedes8: images.mercedes8,
  mercedes9: images.mercedes9,
  mercedes10: images.mercedes10,
  mercedes11: images.mercedes11,
  mercedes12: images.mercedes12,
  // Tesla
  tesla1: images.tesla1,
  tesla2: images.tesla2,
  tesla3: images.tesla3,
  tesla4: images.tesla4,
  tesla5: images.tesla5,
  tesla6: images.tesla6,
  tesla7: images.tesla7,
  tesla8: images.tesla8,
  tesla9: images.tesla9,
  tesla10: images.tesla10,
  tesla11: images.tesla11,
  tesla12: images.tesla12,
  // Toyota
  toyota1: images.toyota1,
  toyota2: images.toyota2,
  toyota3: images.toyota3,
  toyota4: images.toyota4,
  toyota5: images.toyota5,
  toyota6: images.toyota6,
  toyota7: images.toyota7,
  toyota8: images.toyota8,
  toyota9: images.toyota9,
  toyota10: images.toyota10,
  toyota11: images.toyota11,
  toyota12: images.toyota12,
  // Volvo
  volvo1: images.volvo1,
  volvo2: images.volvo2,
  volvo3: images.volvo3,
  volvo4: images.volvo4,
  volvo5: images.volvo5,
  volvo6: images.volvo6,
  volvo7: images.volvo7,
  volvo8: images.volvo8,
  volvo9: images.volvo9,
  volvo10: images.volvo10,
  volvo11: images.volvo11,
  volvo12: images.volvo12,
};

const BRAND_LOGO_MAP: Record<string, any> = {
  bmw: icons.bmw,
  bugatti: icons.bugatti,
  honda: icons.honda,
  mercedes: icons.mercedes,
  tesla: icons.tesla,
  toyota: icons.toyota,
  volvo: icons.volvo,
};

export function resolveCarImage(key: string): any {
  if (key?.startsWith('http') || key?.startsWith('/')) {
    return { uri: normalizeRemoteImageUri(key) };
  }

  return CAR_IMAGE_MAP[key] ?? images.honda1;
}

export function resolveBrandLogo(key: string): any {
  return BRAND_LOGO_MAP[key] ?? icons.category;
}

export function resolveCategoryIcon(imageUrl?: string | null, logoKey?: string): any {
  if (logoKey && logoKey.trim().length > 0 && BRAND_LOGO_MAP[logoKey.trim()]) {
    return resolveBrandLogo(logoKey.trim());
  }

  if (imageUrl && imageUrl.trim().length > 0) {
    return { uri: normalizeRemoteImageUri(imageUrl.trim()) };
  }

  if (logoKey && logoKey.trim().length > 0) {
    return resolveBrandLogo(logoKey.trim());
  }

  return icons.category;
}

export function resolveSliderImages(keys: string[]): any[] {
  return keys.map(k => resolveCarImage(k));
}

function normalizeRemoteImageUri(value: string): string {
  if (!value) {
    return value;
  }

  if (value.startsWith('/')) {
    return `${API_ORIGIN}${value}`;
  }

  try {
    const url = new URL(value);
    if (Platform.OS === 'android' && (url.hostname === 'localhost' || url.hostname === '127.0.0.1')) {
      url.hostname = '10.0.2.2';
    }

    return url.toString();
  } catch {
    return value;
  }
}
