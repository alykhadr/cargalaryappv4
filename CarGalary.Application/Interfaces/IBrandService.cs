
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Dtos.Brand.Query;
using CarGalary.Application.Dtos.CarModel.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandResponseDto?> GetByIdAsync(int id);
        Task<List<CarModelByBrandResponseDto>> GetCarModelsByBrandAsync(int brandId);
      
        Task<BrandResponseDto> CreateAsync(CreateBrandRequestDto dto);
        Task UpdateAsync(int id, UpdateBrandRequestDto dto);
        Task DeleteAsync(int id);

    }
}
