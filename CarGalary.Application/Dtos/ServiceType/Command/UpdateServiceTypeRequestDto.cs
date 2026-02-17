namespace CarGalary.Application.Dtos.ServiceType.Command
{
    public class UpdateServiceTypeRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
