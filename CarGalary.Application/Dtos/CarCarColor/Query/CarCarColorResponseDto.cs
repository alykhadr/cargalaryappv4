namespace CarGalary.Application.Dtos.CarCarColor.Query
{
    public class CarCarColorResponseDto
    {
        public int CarId { get; set; }
        public int ColorId { get; set; }
        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }
        public bool IsAvailable { get; set; }
    }
}

