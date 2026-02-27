

namespace CarGalary.Domain.Entities
{
    public class CarColor :BaseEntity
    {
        public const int DiscountTypePercentage = 0;
        public const int DiscountTypeFixedAmount = 1;

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }
       

        public decimal? PricePefore { get; set; }
       
        public decimal? VatAmount { get; set; }
        public decimal? Discount { get; set; }
        // DiscountType: 0 for percentage, 1 for fixed amount
        public int? DiscountType { get; set; }
        public decimal? TotalPrice { get; set; }

        public void ApplyPricing(
            decimal pricingPerColor,
            decimal pricePefore,
            decimal vat,
            decimal discount,
            int discountType)
        {
            if (pricingPerColor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pricingPerColor), "PricingPerColor must be zero or greater.");
            }

            if (pricePefore < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pricePefore), "PricePefore must be zero or greater.");
            }

            if (vat < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(vat), "Vat must be zero or greater.");
            }

            if (discount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(discount), "Discount must be zero or greater.");
            }

            if (discountType != DiscountTypePercentage && discountType != DiscountTypeFixedAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(discountType), "DiscountType must be 0 (percentage) or 1 (fixed amount).");
            }

            PricingPerColor = pricingPerColor;
            PricePefore = pricePefore;
            Discount = discount;
            DiscountType = discountType;
            TotalPrice = CalculateTotalPrice(vat);
        }

        private decimal CalculateTotalPrice(decimal vat)
        {
            var basePrice = PricePefore ?? PricingPerColor ?? 0m;
            var discountValue = Discount ?? 0m;
            var discountType = DiscountType ?? DiscountTypePercentage;

            var discountAmount = discountType == DiscountTypePercentage
                ? basePrice * (discountValue / 100m)
                : discountValue;

            if (discountAmount > basePrice)
            {
                discountAmount = basePrice;
            }

            var priceAfterDiscount = basePrice - discountAmount;
            VatAmount = Math.Round(priceAfterDiscount * (vat / 100m), 2, MidpointRounding.AwayFromZero);
            var vatAmount = VatAmount ?? 0m;
            var total = priceAfterDiscount + vatAmount;

            return Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }
    }
}
