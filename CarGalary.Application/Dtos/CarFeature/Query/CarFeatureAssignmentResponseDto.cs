namespace CarGalary.Application.Dtos.CarFeature.Query
{
    public class CarFeatureAssignmentResponseDto
    {
        public int CarId { get; set; }
        public int FeatureId { get; set; }
        public bool IsAvailable { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
