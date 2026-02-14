using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Domain.Entities
{
    public class AudioAndCommunicationSystem :BaseEntity
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
         public string? DescriptionAr { get; set; }
        public string? CreatedBy { get; set; }

        // Foreign key
        public int CarId { get; set; }
        public Car? Car { get; set; }  // Navigation property
    }
}