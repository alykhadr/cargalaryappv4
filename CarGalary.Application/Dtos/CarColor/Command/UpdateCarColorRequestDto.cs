namespace CarGalary.Application.Dtos.CarColor.Command
{
    public class UpdateCarColorRequestDto
    {
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
        public bool? IsAvailable { get; set; }
    }
}

