namespace CarGalary.Application.Dtos.Exterior.Command
{
    public class CreateExteriorRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int CarId { get; set; }
    }
}
