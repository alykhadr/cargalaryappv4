namespace CarGalary.Domain.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = default!;

        public int CarId { get; set; }
        public Car Car { get; set; } = default!;

        public int? ColorId { get; set; }
        public Color? Color { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
    }
}
