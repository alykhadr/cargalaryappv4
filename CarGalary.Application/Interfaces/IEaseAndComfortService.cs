using CarGalary.Application.Dtos.EaseAndComfort.Command;
using CarGalary.Application.Dtos.EaseAndComfort.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IEaseAndComfortService
    {
        Task<List<EaseAndComfortResponseDto>> GetAllAsync();
        Task<EaseAndComfortResponseDto?> GetByIdAsync(int id);
        Task<EaseAndComfortResponseDto> CreateAsync(CreateEaseAndComfortRequestDto dto);
        Task UpdateAsync(int id, UpdateEaseAndComfortRequestDto dto);
        Task DeleteAsync(int id);
    }
}
