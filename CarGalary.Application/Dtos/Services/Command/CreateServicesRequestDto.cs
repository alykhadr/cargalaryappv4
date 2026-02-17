namespace CarGalary.Application.Dtos.Services.Command
{
    public class CreateServicesRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal? Discount { get; set; }
        public bool IsPercentage { get; set; } = true;
        public string? ServiceImageUrl { get; set; }
        public int ServiceTypeId { get; set; }
    }
}
