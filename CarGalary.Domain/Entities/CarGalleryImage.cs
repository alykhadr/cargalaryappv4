

namespace CarGalary.Domain.Entities
{
   public class CarGalleryImage:BaseEntity
{
   
    public int CarId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int? ImageType { get; set; }
    public bool IsPrimary { get; set; } = false;

    public string? CreatedBy { get; set; }
    

    // Navigation
    public Car Car { get; set; } = null!;
}
}