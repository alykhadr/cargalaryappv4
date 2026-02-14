using CarGalary.Application.Dtos.CarColor.Command;
using CarGalary.Application.Dtos.CarColor.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarColorService
    {
        Task<List<CarColorResponseDto>> GetAllAsync();
        Task<CarColorResponseDto?> GetByIdAsync(int id);

        Task<CarColorResponseDto> CreateAsync(CreateCarColorRequestDto dto);
        Task UpdateAsync(int id, UpdateCarColorRequestDto dto);
        Task DeleteAsync(int id);
    }
}

