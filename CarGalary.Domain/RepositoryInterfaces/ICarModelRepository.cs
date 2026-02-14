using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarModelRepository
    {
        Task<List<CarModel>> GetAllAsync();
        Task<CarModel?> GetByIdAsync(int id);
        Task CreateAsync(CarModel model);
        Task UpdateAsync(CarModel model);
        Task DeleteAsync(CarModel model);
    }
}

