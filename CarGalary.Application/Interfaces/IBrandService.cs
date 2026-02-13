
using CarGalary.Application.Dtos.Brand;

namespace CarGalary.Application.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        //   Task<CarBrand> GetBrandById(int id);

        Task CreateAsync(BrandDto dto);

        //  Task<bool> UpdateBrand(int id, CarBrand brand) ;
        //  Task<bool>  BrandExists(int id);

        //  Task<bool> DeleteBrandById(int id);
    }
}