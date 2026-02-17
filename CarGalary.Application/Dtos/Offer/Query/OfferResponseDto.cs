namespace CarGalary.Application.Dtos.Offer.Query
{
    public class OfferResponseDto
    {
        public int Id { get; set; }
        public string? OfferImageUrl { get; set; }
        public string? OfferNameAr { get; set; }
        public string? OfferNameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool IsAvailable { get; set; }
    }
}
