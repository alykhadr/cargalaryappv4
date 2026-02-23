using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.CarGalleryImage.Command
{
    public class UpdateCarGalleryImageRequestDto
    {
        public int CarId { get; set; }
        public string? ImageUrl { get; set; }
        public int? ImageType { get; set; }
        public bool IsPrimary { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
