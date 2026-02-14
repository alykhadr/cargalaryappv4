using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Brand.Command
{
    public class CreateBrandRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
