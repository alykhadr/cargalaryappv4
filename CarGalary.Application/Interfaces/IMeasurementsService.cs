using CarGalary.Application.Dtos.Measurements.Command;
using CarGalary.Application.Dtos.Measurements.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IMeasurementsService
    {
        Task<List<MeasurementsResponseDto>> GetAllAsync();
        Task<MeasurementsResponseDto?> GetByIdAsync(int id);
        Task<MeasurementsResponseDto> CreateAsync(CreateMeasurementsRequestDto dto);
        Task UpdateAsync(int id, UpdateMeasurementsRequestDto dto);
        Task DeleteAsync(int id);
    }
}
