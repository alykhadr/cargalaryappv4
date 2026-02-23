namespace CarGalary.Application.Dtos.ContactUs.Query
{
    public class ContactUsResponseDto
    {
        public int Id { get; set; }
        public string? ContactValue { get; set; }
        public int ContactType { get; set; }
        public string? ContactIconUrl { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
