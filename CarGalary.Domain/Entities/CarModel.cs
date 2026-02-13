



namespace CarGalary.Domain.Entities
{
    public class CarModel:BaseEntity
    {

        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? CreatedBy { get; set; }


        public int BrandId { get; set; }
        public CarBrand Brand { get; set; } = null!;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}