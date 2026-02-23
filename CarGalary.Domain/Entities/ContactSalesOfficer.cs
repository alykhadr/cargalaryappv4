

namespace CarGalary.Domain.Entities
{
    public class ContactSalesOfficer:BaseEntity
    {

        public string? ContactValue { get; set; }
        public bool IsMobileNo { get; set; }=true;
        public string? CreatedBy { get; set; }
    }
}