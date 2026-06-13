namespace CarGalary.Application.Dtos.Invoice.Query
{
    public class InvoiceCreateResultDto
    {
        public InvoiceResponseDto Invoice { get; set; } = new();
        public List<InvoiceLowStockAlertDto> LowStockAlerts { get; set; } = new();
    }

    public class InvoiceLowStockAlertDto
    {
        public int CarId { get; set; }
        public string? CarNameAr { get; set; }
        public string? CarNameEn { get; set; }
        public int? ColorId { get; set; }
        public string? ColorNameAr { get; set; }
        public string? ColorNameEn { get; set; }
        public int RemainingStockQuantity { get; set; }
        public int ThresholdQuantity { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
    }
}
