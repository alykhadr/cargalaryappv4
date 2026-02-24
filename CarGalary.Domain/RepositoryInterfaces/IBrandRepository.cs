

using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetBrands();
        Task<Brand?> GetBrandById(int id);
        Task<List<CarModel>> GetCarModelsByBrandAsync(int brandId);

        Task CreateBrand(Brand brand);

        Task UpdateBrand(Brand brand);
        Task<Brand?> BrandExists(int id);

        Task DeleteBrandById(Brand brand);
    }
}
