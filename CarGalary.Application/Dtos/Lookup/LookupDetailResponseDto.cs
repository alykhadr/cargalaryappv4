namespace CarGalary.Application.Dtos.Lookup
{
    public class LookupDetailResponseDto
    {
        public int Id { get; set; }
        public string MasterCode { get; set; } = string.Empty;
        public string DetailCode { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DisplayName => $"{NameAr} - {NameEn}";
    }
}
