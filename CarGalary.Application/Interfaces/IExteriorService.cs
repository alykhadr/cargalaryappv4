using CarGalary.Application.Dtos.Exterior.Command;
using CarGalary.Application.Dtos.Exterior.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IExteriorService
    {
        Task<List<ExteriorResponseDto>> GetAllAsync();
        Task<ExteriorResponseDto?> GetByIdAsync(int id);
        Task<ExteriorResponseDto> CreateAsync(CreateExteriorRequestDto dto);
        Task UpdateAsync(int id, UpdateExteriorRequestDto dto);
        Task DeleteAsync(int id);
    }
}
