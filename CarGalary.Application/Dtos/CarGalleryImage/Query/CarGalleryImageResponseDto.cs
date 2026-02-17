namespace CarGalary.Application.Dtos.CarGalleryImage.Query
{
    public class CarGalleryImageResponseDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string? ImageUrl { get; set; }
        public int? ImageType { get; set; }
        public bool IsPrimary { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
