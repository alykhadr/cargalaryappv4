using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Dtos.Services.Query;

namespace CarGalary.Application.Interfaces
{
   public interface IServicesService
   {
        Task<List<ServicesResponseDto>> GetAllAsync();
        Task<ServicesResponseDto?> GetByIdAsync(int id);
        Task<ServicesResponseDto> CreateAsync(CreateServicesRequestDto dto);
        Task UpdateAsync(int id, UpdateServicesRequestDto dto);
        Task DeleteAsync(int id);
   }
}
