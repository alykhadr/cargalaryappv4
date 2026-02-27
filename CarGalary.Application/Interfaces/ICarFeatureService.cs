

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
        Task<List<CarFeatureAssignmentResponseDto>> GetAssignmentsByCarIdAsync(int carId);
        Task<CarFeatureAssignmentResponseDto> CreateAssignmentAsync(int carId, CreateCarFeatureAssignmentRequestDto dto);
        Task UpdateAssignmentAsync(int carId, int featureId, UpdateCarFeatureAssignmentRequestDto dto);
        Task DeleteAssignmentAsync(int carId, int featureId);
    }
}
