using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.CarCarColor.Command
{
    public class UpdateCarCarColorRequestDto
    {
        public int? StockQuantity { get; set; }
        public string? ColorImageUrl { get; set; }
        public IFormFile? ColorImageFile { get; set; }
        public decimal? PricingPerColor { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
