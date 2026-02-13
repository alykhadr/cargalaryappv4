

namespace CarGalary.Domain.Entities
{
    public class CarColor:BaseEntity
    {

        public string? ColorNameAr { get; set; }
         public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
       
        public string? CreatedBy { get; set; }
         // Navigation property for many-to-many
    public ICollection<CarCarColor> Cars { get; set; } = new List<CarCarColor>();
    }
}