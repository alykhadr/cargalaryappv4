

namespace CarGalary.Domain.Entities
{
    public class Seating:BaseEntity
    {
  
         public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
         public string? DescriptionEn { get; set; }
       
        public string? CreatedBy { get; set; }
       

        // Foreign key
        public int CarId { get; set; }
        public Car? Car { get; set; }  // Navigation property
    }
}