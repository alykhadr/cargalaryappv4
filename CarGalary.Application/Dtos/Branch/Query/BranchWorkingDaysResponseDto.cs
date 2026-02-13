using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Dtos.Branch
{
    public class BranchWorkingDaysResponseDto
    {

        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public string? DayAr { get; set; }
        public string? DayEn { get; set; }
        public int? WorkingFrom { get; set; }
        public int? WorkingTo { get; set; }
        public string? TimeType { get; set; }
    }
}