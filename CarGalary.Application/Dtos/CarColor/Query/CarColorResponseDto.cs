namespace CarGalary.Application.Dtos.CarColor.Query
{
    public class CarColorResponseDto
    {
        public int Id { get; set; }
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}

