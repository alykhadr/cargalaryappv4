using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.MemberService.Command
{
    public class UpdateMemberServiceRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsAvailable { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
