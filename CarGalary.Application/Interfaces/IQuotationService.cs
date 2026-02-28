using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Quotation.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IQuotationService
    {
        Task<List<QuotationResponseDto>> GetAllAsync();
        Task<QuotationResponseDto> GetByIdAsync(int id);
        Task<QuotationResponseDto> CreateAsync(CreateQuotationRequestDto dto);
        Task<QuotationResponseDto> UpdateStatusAsync(int quotationId, UpdateQuotationStatusRequestDto dto);
        Task<List<QuotationHistoryResponseDto>> GetHistoryAsync(int quotationId);
    }
}
