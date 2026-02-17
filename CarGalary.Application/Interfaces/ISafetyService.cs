using CarGalary.Application.Dtos.Safety.Command;
using CarGalary.Application.Dtos.Safety.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ISafetyService
    {
        Task<List<SafetyResponseDto>> GetAllAsync();
        Task<SafetyResponseDto?> GetByIdAsync(int id);
        Task<SafetyResponseDto> CreateAsync(CreateSafetyRequestDto dto);
        Task UpdateAsync(int id, UpdateSafetyRequestDto dto);
        Task DeleteAsync(int id);
    }
}
