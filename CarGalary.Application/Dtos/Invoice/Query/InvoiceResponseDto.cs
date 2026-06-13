namespace CarGalary.Application.Dtos.Invoice.Query
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserFullNameAr { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserEmail { get; set; }
        public int BranchId { get; set; }
        public string? BranchNameAr { get; set; }
        public string? BranchNameEn { get; set; }
        public int PaymentMethod { get; set; }
        public string? PaymentMethodNameAr { get; set; }
        public string? PaymentMethodNameEn { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable { get; set; }
        public List<InvoiceDetailResponseDto> Details { get; set; } = new();
    }

    public class InvoiceDetailResponseDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string? CarNameAr { get; set; }
        public string? CarNameEn { get; set; }
        public string? ModelNameAr { get; set; }
        public string? ModelNameEn { get; set; }
        public string? BrandNameAr { get; set; }
        public string? BrandNameEn { get; set; }
        public string? PlateNumberAr { get; set; }
        public string? PlateNumberEn { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public int? ColorId { get; set; }
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public string? ColorCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}
