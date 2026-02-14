namespace CarGalary.Application.Dtos.CarModel.Query
{
    public class CarModelResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int BrandId { get; set; }
    }
}
