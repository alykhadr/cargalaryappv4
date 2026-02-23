using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarCarColorRepository
    {
        Task<List<CarColor>> GetAllAsync();
        Task<CarColor?> GetByIdAsync(int carId, int colorId);
        Task<List<CarColor>> GetByCarIdAsync(int carId);

        Task CreateAsync(CarColor entity);
        Task UpdateAsync(CarColor entity);
        Task DeleteAsync(CarColor entity);
    }
}

