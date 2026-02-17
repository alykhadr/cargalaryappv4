using CarGalary.Application.Dtos.ExtraFeature.Command;
using CarGalary.Application.Dtos.ExtraFeature.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IExtraFeatureService
    {
        Task<List<ExtraFeatureResponseDto>> GetAllAsync();
        Task<ExtraFeatureResponseDto?> GetByIdAsync(int id);
        Task<ExtraFeatureResponseDto> CreateAsync(CreateExtraFeatureRequestDto dto);
        Task UpdateAsync(int id, UpdateExtraFeatureRequestDto dto);
        Task DeleteAsync(int id);
    }
}
