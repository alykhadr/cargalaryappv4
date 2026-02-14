namespace CarGalary.Application.Dtos.Brand.Query
{
    public class BrandResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? ImageUrl { get; set; }
        public string? CreatedBy { get; set; }
    }
}
