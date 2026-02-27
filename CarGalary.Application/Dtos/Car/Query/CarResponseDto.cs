namespace CarGalary.Application.Dtos.Car.Query
{
    public class CarResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int ModelId { get; set; }
        public int TypeId { get; set; }
        public int BranchId { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsAvailable { get; set; }
    }
}
