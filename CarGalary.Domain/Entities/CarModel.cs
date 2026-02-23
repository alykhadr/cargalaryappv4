



namespace CarGalary.Domain.Entities
{
    public class CarModel:BaseEntity
    {

        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ImageUrl { get; set; }


        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}