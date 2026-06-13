namespace CarGalary.Application.Dtos.Invoice.Command
{
    public class UpdateInvoiceRequestDto
    {
        public Guid? UserId { get; set; }
        public int BranchId { get; set; }
        public int PaymentMethod { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Notes { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal ExtraDiscount { get; set; }
        public bool? IsAvailable { get; set; }
        public List<UpdateInvoiceDetailRequestDto> Details { get; set; } = new();
    }

    public class UpdateInvoiceDetailRequestDto
    {
        public int CarId { get; set; }
        public int? ColorId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string? Notes { get; set; }
    }
}
