
using CarGalary.Domain.Entities;


public class ContactUs : BaseEntity
{

    public string? ContactValue { get; set; }
    // mobile , whatsup , email
    public int ContactType { get; set; }
    public string? ContactIconUrl { get; set; }
    public string? MessageAr { get; set; }
    public string? MessageEn { get; set; }

    public string? CreatedBy { get; set; }
}

