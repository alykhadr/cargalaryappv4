using CarGalary.Domain.Entities;


public class CarType:BaseEntity
{
    public string? NameAr { get; set; } 
    public string? NameEn { get; set; } 
    public string? CreatedBy { get; set; }

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}
