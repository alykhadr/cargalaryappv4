

namespace CarGalary.Domain.Entities
{
    public class CarCarColor
    {
        public int CarId { get; set; }
         public Car Car { get; set; } = null!;
        public int ColorId { get; set; }
         public Color CarColor { get; set; } = null!;

        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; } = true;

    }
}