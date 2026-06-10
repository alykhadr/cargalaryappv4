import React from 'react';
import { Image, Text } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { COLORS } from '../constants';

interface AppAvatarProps {
  uri?: string | null;
  size?: number;
}

const AppAvatar: React.FC<AppAvatarProps> = ({ uri, size = 50 }) => {
  // iOS-style squircle radius (~22% of size)
  const radius = Math.round(size * 0.22);
  const fontSize = Math.round(size * 0.48);

  if (uri) {
    return (
      <Image
        source={{ uri }}
        resizeMode="cover"
        style={{ width: size, height: size, borderRadius: radius }}
      />
    );
  }

  return (
    <LinearGradient
      colors={['#FF6B35', '#E8001C']}
      start={{ x: 0, y: 0 }}
      end={{ x: 1, y: 1 }}
      style={{
        width: size,
        height: size,
        borderRadius: radius,
        alignItems: 'center',
        justifyContent: 'center',
        shadowColor: '#E8001C',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.35,
        shadowRadius: 8,
        elevation: 6,
      }}
    >
      <Text
        style={{
          fontSize,
          fontFamily: 'extraBold',
          color: '#FFFFFF',
          lineHeight: fontSize * 1.15,
          letterSpacing: -1,
          includeFontPadding: false,
        }}
      >
        C
      </Text>
    </LinearGradient>
  );
};

export default AppAvatar;
