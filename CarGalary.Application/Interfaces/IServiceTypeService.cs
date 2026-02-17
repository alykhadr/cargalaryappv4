using CarGalary.Application.Dtos.ServiceType.Command;
using CarGalary.Application.Dtos.ServiceType.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IServiceTypeService
    {
        Task<List<ServiceTypeResponseDto>> GetAllAsync();
        Task<ServiceTypeResponseDto?> GetByIdAsync(int id);
        Task<ServiceTypeResponseDto> CreateAsync(CreateServiceTypeRequestDto dto);
        Task UpdateAsync(int id, UpdateServiceTypeRequestDto dto);
        Task DeleteAsync(int id);
    }
}
