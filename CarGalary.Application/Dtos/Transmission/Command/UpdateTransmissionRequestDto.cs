namespace CarGalary.Application.Dtos.Transmission.Command
{
    public class UpdateTransmissionRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int CarId { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
