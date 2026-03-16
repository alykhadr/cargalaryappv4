using CarGalary.Application.Dtos.Package.Command;
using CarGalary.Application.Dtos.Package.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IPackageService
    {
        Task<List<PackageResponseDto>> GetAllAsync();
        Task<PackageResponseDto?> GetByIdAsync(int id);
        Task<PackageResponseDto> CreateAsync(CreatePackageRequestDto dto);
        Task UpdateAsync(int id, UpdatePackageRequestDto dto);
        Task DeleteAsync(int id);
    }
}
