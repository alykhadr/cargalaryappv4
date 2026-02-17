using CarGalary.Application.Dtos.EngineSpecification.Command;
using CarGalary.Application.Dtos.EngineSpecification.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IEngineSpecificationService
    {
        Task<List<EngineSpecificationResponseDto>> GetAllAsync();
        Task<EngineSpecificationResponseDto?> GetByIdAsync(int id);
        Task<EngineSpecificationResponseDto> CreateAsync(CreateEngineSpecificationRequestDto dto);
        Task UpdateAsync(int id, UpdateEngineSpecificationRequestDto dto);
        Task DeleteAsync(int id);
    }
}
