namespace CarGalary.Application.Dtos.CarCarColor.Query
{
    public class CarCarColorResponseDto
    {
        public int CarId { get; set; }
        public int ColorId { get; set; }
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
        public int ColorStatus { get; set; }
        public string? ColorStatusDetailCode { get; set; }
        public string? ColorStatusNameAr { get; set; }
        public string? ColorStatusNameEn { get; set; }
        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public decimal? PricingPerColor { get; set; }
        public decimal? PricePefore { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountType { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
