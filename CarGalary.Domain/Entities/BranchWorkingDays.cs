
using CarGalary.Domain.Entities;

namespace CarGalary.Domain.Entities
{
    public class BranchWorkingDays:BaseEntity
    {
        public string? DayAr { get; set; }
         public string? DayEn { get; set; }
        public int? WorkingFrom { get; set; }
        public int? WorkingTo { get; set; }
        public string? TimeType { get; set; }
        public string? CreatedBy { get; set; }

         public int  BranchId { get; set; }
        public Branchs?  Branch { get; set; } 

        
        
    }
}