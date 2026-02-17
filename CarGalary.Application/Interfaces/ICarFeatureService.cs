

using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.CarFeature.Query;

namespace CarGalary.Application.Interfaces
{
    public interface ICarFeatureService
    {
        Task<List<CarFeatureResponseDto>> GetAllAsync();
        Task<CarFeatureResponseDto?> GetByIdAsync(int id);
        Task<CarFeatureResponseDto> CreateAsync(CreateCarFeatureRequestDto dto);
        Task UpdateAsync(int id, UpdateCarFeatureRequestDto dto);
        Task DeleteAsync(int id);
    }
}
