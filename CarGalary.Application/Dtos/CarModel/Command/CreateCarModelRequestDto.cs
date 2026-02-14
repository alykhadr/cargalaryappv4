using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.CarModel.Command
{
    public class CreateCarModelRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int BrandId { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
