
namespace CarGalary.Domain.Entities
{
    public class CarExtraDetails :BaseEntity
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
         public string? DescriptionAr { get; set; }

        /// <summary>
        /// AudioAndCommunicationSystem ,EaseAndComfort,EngineSpecification,Exterior,ExtraFeature
        /// Measurements,Safety,Seating,Transmission
        /// </summary>
         public int CarExtraDetailsType { get; set; }
        public string? CreatedBy { get; set; }

        // Foreign key
        public int CarId { get; set; }
        public Car? Car { get; set; }  // Navigation property
    }
}