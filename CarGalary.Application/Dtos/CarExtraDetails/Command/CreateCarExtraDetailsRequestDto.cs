namespace CarGalary.Application.Dtos.CarExtraDetails.Command
{
    public class CreateCarExtraDetailsRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public int CarId { get; set; }
    }
}

