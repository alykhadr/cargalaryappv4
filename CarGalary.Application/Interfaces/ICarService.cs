
using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Car.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarService
    {
        Task<List<CarResponseDto>> GetAllAsync();
        Task<CarResponseDto?> GetByIdAsync(int id);
        Task<CarResponseDto> CreateAsync(CreateCarRequestDto dto);
        Task UpdateAsync(int id, UpdateCarRequestDto dto);
        Task DeleteAsync(int id);
        Task<List<CarResponseDto>> FilterAsync(int? modelId = null, int? typeId = null, bool? isAvailable = null);
    }
}
