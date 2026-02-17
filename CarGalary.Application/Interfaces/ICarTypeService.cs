

using CarGalary.Application.Dtos.CarType.Command;
using CarGalary.Application.Dtos.CarType.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarTypeService
    {
        Task<List<CarTypeResponseDto>> GetAllAsync();
        Task<CarTypeResponseDto?> GetByIdAsync(int id);
        Task<CarTypeResponseDto> CreateAsync(CreateCarTypeRequestDto dto);
        Task UpdateAsync(int id, UpdateCarTypeRequestDto dto);
        Task DeleteAsync(int id);
    }
}
