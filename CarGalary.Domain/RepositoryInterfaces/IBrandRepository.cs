

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IBrandRepository
    {
        Task<List<CarBrand>> GetBrands();
        Task<CarBrand?> GetBrandById(int id);

        Task CreateBrand(CarBrand brand);

        Task UpdateBrand(CarBrand brand);
        Task<CarBrand?> BrandExists(int id);

        Task DeleteBrandById(CarBrand brand);
    }
}
