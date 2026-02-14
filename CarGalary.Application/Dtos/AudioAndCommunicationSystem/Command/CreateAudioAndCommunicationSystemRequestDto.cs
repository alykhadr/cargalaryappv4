namespace CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command
{
    public class CreateAudioAndCommunicationSystemRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public int CarId { get; set; }
    }
}

