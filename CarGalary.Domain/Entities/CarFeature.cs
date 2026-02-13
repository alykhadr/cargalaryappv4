




using CarGalary.Domain.Entities;

public class CarFeature:BaseEntity
{
    public string? NameAr { get; set; }
     public string? NameEn { get; set; }

    public ICollection<CarCarFeature> CarCarFeatures { get; set; }
        = new List<CarCarFeature>();
}