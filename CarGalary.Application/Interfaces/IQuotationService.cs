using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Quotation.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IQuotationService
    {
        Task<List<QuotationResponseDto>> GetAllAsync();
        Task<QuotationResponseDto> CreateAsync(CreateQuotationRequestDto dto);
    }
}
