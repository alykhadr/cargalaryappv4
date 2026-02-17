namespace CarGalary.Application.Dtos.Exterior.Query
{
    public class ExteriorResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? CreatedBy { get; set; }
        public int CarId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
