

namespace CarGalary.Domain.Entities
{
   public class CarGalleryImage
{
    public int ImageId { get; set; }
    public int CarId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int? ImageType { get; set; }
    public bool IsPrimary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Car Car { get; set; } = null!;
}
}