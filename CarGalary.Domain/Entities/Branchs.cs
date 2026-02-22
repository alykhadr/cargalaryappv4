

namespace CarGalary.Domain.Entities
{
    public class Branchs:BaseEntity
    {
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? MobileNo  { get; set; }
        public string? Email { get; set; }
        public string? BranchNameAr { get; set; }
          public string? BranchNameEn { get; set; }
        public string? CreatedBy { get; set; }
        public string? Address { get; set; }
        public string? WhatsUpNo { get; set; }
        public string? Latitute { get; set; }
        public string? Longtute { get; set; }

        public ICollection<BranchWorkingDays>     BranchWorkingDays { get; set; }=new List<BranchWorkingDays>();
        public ICollection<Car>   Cars    { get; set; }=new List<Car>();
                public ICollection<ApplicationUser>    ApplicationUsers    { get; set; }=new List<ApplicationUser>();
    }
}