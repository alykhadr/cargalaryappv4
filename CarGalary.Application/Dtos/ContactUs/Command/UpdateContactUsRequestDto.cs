namespace CarGalary.Application.Dtos.ContactUs.Command
{
    public class UpdateContactUsRequestDto
    {
        public string? UserId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
