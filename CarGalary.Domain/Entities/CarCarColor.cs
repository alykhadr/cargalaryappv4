

using CarGalary.Domain.Entities;

namespace CarGalary.Domain.Entities
{
    public class CarCarColor:BaseEntity
    {
        public int CarId { get; set; }
         public Car Car { get; set; } = null!;
        public int ColorId { get; set; }
         public CarColor CarColor { get; set; } = null!;

        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }

    }
}