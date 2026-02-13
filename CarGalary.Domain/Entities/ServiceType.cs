

namespace CarGalary.Domain.Entities
{
    public class ServiceType:BaseEntity
    {
       
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
         public string? DescriptionEn { get; set; }
       

          // Navigation (1 -> many)
    public ICollection<Services> Services { get; set; } = new List<Services>();
    }
}