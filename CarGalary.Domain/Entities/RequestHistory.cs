namespace CarGalary.Domain.Entities
{
    public class RequestHistory : BaseEntity
    {
        public int RequestId { get; set; }
        public Request Request { get; set; } = default!;

        public int Status { get; set; }
        public LookupDetails? StatusLookup { get; set; }

        public DateTime StatusDate { get; set; }
        public string? Notes { get; set; }
    }
}
