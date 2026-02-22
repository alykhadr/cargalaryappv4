namespace CarGalary.Application.Dtos.Branch.Command
{
    public class UpdateBranchRequestDto
    {
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public string? Address { get; set; }
        public string? WhatsUpNo { get; set; }
        public string? Latitute { get; set; }
        public string? Longtute { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
