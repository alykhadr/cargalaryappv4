namespace CarGalary.Application.Dtos.Services.Command
{
    public class BulkDeleteServicesRequestDto
    {
        public List<int> ServiceIds { get; set; } = new();
    }
}
