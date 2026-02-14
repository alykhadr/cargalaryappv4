using CarGalary.Application.Dtos.CarCarColor.Command;
using CarGalary.Application.Dtos.CarCarColor.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarCarColorService
    {
        Task<List<CarCarColorResponseDto>> GetAllAsync();
        Task<CarCarColorResponseDto?> GetByIdAsync(int carId, int colorId);
        Task<List<CarCarColorResponseDto>> GetByCarIdAsync(int carId);

        Task<CarCarColorResponseDto> CreateAsync(CreateCarCarColorRequestDto dto);
        Task UpdateAsync(int carId, int colorId, UpdateCarCarColorRequestDto dto);
        Task DeleteAsync(int carId, int colorId);
    }
}

