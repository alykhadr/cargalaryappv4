using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarCarColorRepository
    {
        Task<List<CarCarColor>> GetAllAsync();
        Task<CarCarColor?> GetByIdAsync(int carId, int colorId);
        Task<List<CarCarColor>> GetByCarIdAsync(int carId);

        Task CreateAsync(CarCarColor entity);
        Task UpdateAsync(CarCarColor entity);
        Task DeleteAsync(CarCarColor entity);
    }
}

