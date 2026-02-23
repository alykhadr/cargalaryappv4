

namespace CarGalary.Domain.Entities
{
    public class Color:BaseEntity
    {

        public string? ColorNameAr { get; set; }
         public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
       
        public string? CreatedBy { get; set; }
        
    public ICollection<CarCarColor> Cars { get; set; } = new List<CarCarColor>();
    }
}