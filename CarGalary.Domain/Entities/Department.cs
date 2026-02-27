namespace CarGalary.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
