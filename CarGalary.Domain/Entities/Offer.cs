

namespace CarGalary.Domain.Entities
{
    public class Offer:BaseEntity
    {

        public string? OfferImageUrl { get; set; }
        public string? OfferNameAr { get; set; }
        public string? OfferNameEn { get; set; }
        public string? DescriptionAr { get; set; }
         
        public string? DescriptionEn { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool IsPercentage { get; set; }
        public decimal? PercentageValue { get; set; }
       
    }
}
