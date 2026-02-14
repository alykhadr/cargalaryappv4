
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Dtos.Brand.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandResponseDto?> GetByIdAsync(int id);

      
        Task<BrandResponseDto> CreateAsync(CreateBrandRequestDto dto);
        Task UpdateAsync(int id, UpdateBrandRequestDto dto);
        Task DeleteAsync(int id);

    }
}
