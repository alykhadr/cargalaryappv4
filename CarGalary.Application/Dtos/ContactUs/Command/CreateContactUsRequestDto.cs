using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.ContactUs.Command
{
    public class CreateContactUsRequestDto
    {
        public string? ContactValue { get; set; }
        public int ContactType { get; set; }
        public IFormFile? IconFile { get; set; }
        public string? ContactIconUrl { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
