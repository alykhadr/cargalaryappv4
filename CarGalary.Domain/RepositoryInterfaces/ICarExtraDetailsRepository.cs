using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ICarExtraDetailsRepository
    {
        Task<List<CarExtraDetails>> GetAllAsync();
        Task<CarExtraDetails?> GetByIdAsync(int id);
        Task<List<CarExtraDetails>> GetByCarIdAsync(int carId);

        Task CreateAsync(CarExtraDetails entity);
        Task UpdateAsync(CarExtraDetails entity);
        Task DeleteAsync(CarExtraDetails entity);
    }
}

