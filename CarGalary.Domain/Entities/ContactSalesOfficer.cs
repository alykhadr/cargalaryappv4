

namespace CarGalary.Domain.Entities
{
    public class ContactSalesOfficer:BaseEntity
    {

        public string? ContactValue { get; set; }
        // mobile , whatsup , email
        public int ContactType { get; set; }
        public string? ContactIconUrl { get; set; }
        public string? CreatedBy { get; set; }
    }
}