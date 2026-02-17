namespace CarGalary.Application.Dtos.FAQ.Command
{
    public class CreateFAQRequestDto
    {
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int Order { get; set; }
    }
}
