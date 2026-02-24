using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Services.Command
{
    public class UpdateServicesRequestDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public decimal Discount { get; set; } = 0;
        public bool IsPercentage { get; set; } = true;
        public IFormFile? ImageFile { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
