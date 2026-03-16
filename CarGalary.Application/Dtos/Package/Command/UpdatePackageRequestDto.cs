using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.Package.Command
{
    public class UpdatePackageRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsAvailable { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
