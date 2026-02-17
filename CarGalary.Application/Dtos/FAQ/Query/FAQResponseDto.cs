namespace CarGalary.Application.Dtos.FAQ.Query
{
    public class FAQResponseDto
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int Order { get; set; }
        public bool IsAvailable { get; set; }
    }
}
