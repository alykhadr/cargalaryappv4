namespace CarGalary.Application.Dtos.CarType.Query
{
    public class CarTypeResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
