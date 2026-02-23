




using CarGalary.Domain.Entities;

public class Feature:BaseEntity
{
     public string? NameAr { get; set; }
     public string? NameEn { get; set; }

    public ICollection<CarFeature> CarFeatures { get; set; }
        = new List<CarFeature>();
}