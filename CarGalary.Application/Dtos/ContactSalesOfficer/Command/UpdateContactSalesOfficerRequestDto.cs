using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Dtos.ContactSalesOfficer.Command
{
    public class UpdateContactSalesOfficerRequestDto
    {
        public string? ContactValue { get; set; }
        public int ContactType { get; set; }
        public IFormFile? IconFile { get; set; }
        public string? ContactIconUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int BranchId { get; set; }
    }
}
