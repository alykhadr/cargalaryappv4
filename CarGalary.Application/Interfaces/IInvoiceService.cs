using CarGalary.Application.Dtos.Invoice.Command;
using CarGalary.Application.Dtos.Invoice.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceResponseDto>> GetAllAsync();
        Task<InvoiceResponseDto> GetByIdAsync(int id);
        Task<InvoiceCreateResultDto> CreateWithStockAsync(CreateInvoiceRequestDto dto);
        Task<InvoiceResponseDto> CreateAsync(CreateInvoiceRequestDto dto);
        Task<InvoiceResponseDto> UpdateAsync(int id, UpdateInvoiceRequestDto dto);
        Task DeleteAsync(int id);
    }
}
