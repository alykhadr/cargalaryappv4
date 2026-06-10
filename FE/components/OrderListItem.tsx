import { View, StyleSheet, TouchableOpacity, Image } from 'react-native';
import React from 'react';
import Text from '@/components/LocalizedText';
import { COLORS, SIZES } from '../constants';
import { useTheme } from '../theme/ThemeProvider';
import { FontAwesome } from '@expo/vector-icons';

interface OrderListItemProps {
  name: string;
  image: any; // Adjust the type according to your image source (e.g., ImageSourcePropType)
  price: number | string; // Assuming price is a number
  rating: number; // Assuming rating is a number
  numReviews: number; // Assuming numReviews is a number
  onPress?: () => void; // Function to handle press event
  size?: string; // Optional size prop
  color: string; // Color should be a string (e.g., hex color)
  quantity: number; // Assuming quantity is a number
}

const OrderListItem: React.FC<OrderListItemProps> = ({
  name,
  image,
  price,
  rating,
  numReviews,
  onPress,
  size,
  color,
  quantity,
}) => {
  const { dark } = useTheme();

  return (
    <TouchableOpacity
      onPress={onPress}
      style={[
        styles.container,
        {
          backgroundColor: dark ? COLORS.dark2 : COLORS.white,
        },
      ]}
    >
      <View
        style={[
          styles.imageContainer,
          {
            backgroundColor: dark ? COLORS.dark3 : COLORS.silver,
          },
        ]}
      >
        <Image source={image} resizeMode='cover' style={styles.image} />
      </View>
      <View style={styles.columnContainer}>
        <View style={styles.topViewContainer}>
          <Text
            style={[
              styles.name,
              {
                color: dark ? COLORS.secondaryWhite : COLORS.greyscale900,
              },
            ]}
          >
            {name}
          </Text>
        </View>
        <View style={styles.viewContainer}>
          <View style={[styles.color, { backgroundColor: color }]}></View>
          <FontAwesome name='star' size={14} color='rgb(250, 159, 28)' />
          <Text
            style={[
              styles.location,
              {
                color: dark ? COLORS.greyscale300 : COLORS.grayscale700,
              },
            ]}
          >
            {' '}
            {rating} ({numReviews})
          </Text>
          {size && (
            <Text
              style={[
                styles.location,
                {
                  color: dark ? COLORS.greyscale300 : COLORS.grayscale700,
                },
              ]}
            >
              {' '}
              | Size= {size}{' '}
            </Text>
          )}
        </View>
        <View style={styles.bottomViewContainer}>
          <View style={styles.priceContainer}>
            <Text
              style={[
                styles.price,
                {
                  color: dark ? COLORS.white : COLORS.primary,
                },
              ]}
            >
              ${price}
            </Text>
          </View>
          <View
            style={[
              styles.qtyContainer,
              {
                backgroundColor: dark ? COLORS.dark3 : COLORS.silver,
              },
            ]}
          >
            <Text
              style={[
                styles.qtyNum,
                {
                  color: dark ? COLORS.white : COLORS.primary,
                },
              ]}
            >
              {quantity}
            </Text>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    width: SIZES.width - 32,
    backgroundColor: COLORS.white,
    padding: 6,
    borderRadius: 16,
    marginBottom: 12,
    height: 112,
    alignItems: 'center',
  },
  image: {
    width: 100,
    height: 100,
    borderRadius: 16,
  },
  imageContainer: {
    width: 100,
    height: 100,
    borderRadius: 16,
    backgroundColor: COLORS.silver,
  },
  columnContainer: {
    flexDirection: 'column',
    marginLeft: 12,
    flex: 1,
  },
  name: {
    fontSize: 17,
    fontFamily: 'bold',
    color: COLORS.greyscale900,
    marginVertical: 4,
    marginRight: 40,
  },
  location: {
    fontSize: 14,
    fontFamily: 'regular',
    color: COLORS.grayscale700,
    marginVertical: 4,
  },
  priceContainer: {
    flexDirection: 'column',
    marginVertical: 4,
  },
  bottomViewContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 2,
  },
  viewContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginVertical: 4,
  },
  price: {
    fontSize: 16,
    fontFamily: 'semiBold',
    color: COLORS.primary,
    marginRight: 8,
  },
  color: {
    height: 16,
    width: 16,
    borderRadius: 999,
    marginRight: 8,
  },
  qtyContainer: {
    width: 36,
    height: 36,
    borderRadius: 18,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: COLORS.silver,
    flexDirection: 'row',
  },
  qtyNum: {
    fontSize: 14,
    fontFamily: 'semiBold',
    color: COLORS.primary,
    marginHorizontal: 12,
  },
  topViewContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: SIZES.width - 164,
  },
});

export default OrderListItem;
