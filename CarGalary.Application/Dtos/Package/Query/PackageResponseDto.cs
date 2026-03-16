namespace CarGalary.Application.Dtos.Package.Query
{
    public class PackageResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? ImageUrl { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
