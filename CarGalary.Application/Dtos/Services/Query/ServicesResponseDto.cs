namespace CarGalary.Application.Dtos.Services.Query
{
    public class ServicesResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Discount { get; set; }
        public bool IsPercentage { get; set; }
        public string? ServiceImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
