using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.CompanyInformation.Command
{
    public class CreateCompanyInformationRequestDto
    {
        public string? CompanyNameAr { get; set; }
        public string? CompanyNameEn { get; set; }
        public string? CRNumber { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? MobileNo { get; set; }
        public string? TelNo { get; set; }
        public string? Email { get; set; }
        public string? AboutUsAr { get; set; }
        public string? AboutUsEn { get; set; }
        public string? OurMissionAr { get; set; }
        public string? OurMissionEn { get; set; }
        public string? OurGoalsAr { get; set; }
        public string? OurGoalsEn { get; set; }
    }
}
