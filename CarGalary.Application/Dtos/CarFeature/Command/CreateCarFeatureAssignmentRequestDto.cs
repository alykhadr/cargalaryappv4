namespace CarGalary.Application.Dtos.CarFeature.Command
{
    public class CreateCarFeatureAssignmentRequestDto
    {
        public int FeatureId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
