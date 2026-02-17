using CarGalary.Application.Dtos.FAQ.Command;
using CarGalary.Application.Dtos.FAQ.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IFAQService
    {
        Task<List<FAQResponseDto>> GetAllAsync();
        Task<FAQResponseDto?> GetByIdAsync(int id);
        Task<FAQResponseDto> CreateAsync(CreateFAQRequestDto dto);
        Task UpdateAsync(int id, UpdateFAQRequestDto dto);
        Task DeleteAsync(int id);
    }
}
