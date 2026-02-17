namespace CarGalary.Application.Dtos.Offer.Command
{
    public class UpdateOfferRequestDto
    {
        public string? OfferImageUrl { get; set; }
        public string? OfferNameAr { get; set; }
        public string? OfferNameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
