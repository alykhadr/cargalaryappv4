namespace CarGalary.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int BranchId { get; set; }
        public Branchs Branch { get; set; } = default!;

        public int PaymentMethod { get; set; }
        public LookupDetails? PaymentMethodLookup { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Notes { get; set; }

        public decimal Subtotal { get; set; }
        public decimal VatTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal ExtraDiscount { get; set; }
        public decimal GrandTotal { get; set; }

        public ICollection<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    }
}
