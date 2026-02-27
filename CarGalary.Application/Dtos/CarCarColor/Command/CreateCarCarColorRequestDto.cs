using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.CarCarColor.Command
{
    public class CreateCarCarColorRequestDto
    {
        public int CarId { get; set; }
        public int ColorId { get; set; }
        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public IFormFile? ColorImageFile { get; set; }
        public decimal? PricingPerColor { get; set; }
        public decimal? PricePefore { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountType { get; set; }
    }
}
