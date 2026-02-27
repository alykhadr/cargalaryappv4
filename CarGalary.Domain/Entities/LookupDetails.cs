namespace CarGalary.Domain.Entities
{
    public class LookupDetails : BaseEntity
    {
        public string MasterCode { get; set; } = string.Empty;
        public string DetailCode { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string? MappedCode { get; set; }
        public string? CreatedBy { get; set; }
    }
}
