using CarGalary.Domain.Entities;


public class CarBrand:BaseEntity
{
   
    public string? NameAr { get; set; } 
    public string? NameEn { get; set; } 
    public string? CreatedBy { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
}
