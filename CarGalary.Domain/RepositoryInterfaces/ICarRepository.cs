
using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarRepository
    {

        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(int id);
        Task CreateAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);
        Task<Car> CarExistsAsync(int id);
        Task<List<Car>> FilterAsync(int? brandId = null, int? typeId = null, bool? isAvailable = null);
    }
}