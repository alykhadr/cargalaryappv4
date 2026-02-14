using CarGalary.Application.Dtos.CarModel.Command;
using CarGalary.Application.Dtos.CarModel.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarModelService
    {
        Task<List<CarModelResponseDto>> GetAllAsync();
        Task<CarModelResponseDto?> GetByIdAsync(int id);
        Task<CarModelResponseDto> CreateAsync(CreateCarModelRequestDto dto);
        Task UpdateAsync(int id, UpdateCarModelRequestDto dto);
        Task DeleteAsync(int id);
    }
}

