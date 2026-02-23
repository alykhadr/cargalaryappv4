

namespace CarGalary.Domain.Entities
{

public class Car:BaseEntity
{

    public int ModelId { get; set; }
    public CarModel? CarModel { get; set; } 

    public int TypeId { get; set; }
    public CarType? Type { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string? DescriptionAr { get; set; }
     public string? DescriptionEn { get; set; }
    public string? CreatedBy { get; set; }

     public int BranchId { get; set; }
    public Branchs?  Branchs { get; set; }

    public ICollection<UserFavorite> FavoritedBy { get; set; }
        = new List<UserFavorite>();

        public ICollection<CarFeature> CarFeatures { get; set; }
        = new List<CarFeature>();


        // Navigation property for many-to-many
    public ICollection<CarColor> CarColors { get; set; } = new List<CarColor>();

     // Images
    public ICollection<CarGalleryImage> CarImages { get; set; } = new List<CarGalleryImage>();

    

           public ICollection<CarExtraDetails>    CarExtraDetails { get; set; }=new List<CarExtraDetails>();

}
}
