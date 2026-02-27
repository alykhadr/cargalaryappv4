namespace CarGalary.Application.Dtos.Department.Command
{
    public class CreateDepartmentRequestDto
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
