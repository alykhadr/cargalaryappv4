using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Package.Command
{
    public class CreatePackageRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
