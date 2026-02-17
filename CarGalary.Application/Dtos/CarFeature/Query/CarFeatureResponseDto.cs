namespace CarGalary.Application.Dtos.CarFeature.Query
{
    public class CarFeatureResponseDto
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool IsAvailable { get; set; }
    }
}
