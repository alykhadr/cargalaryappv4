using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IMeasurementsRepository
    {
        Task<IEnumerable<Measurements>> GetAllAsync();
        Task<Measurements> GetByIdAsync(int id);
        Task CreateAsync(Measurements model);
        Task UpdateAsync(Measurements model);
        Task DeleteAsync(Measurements model);
    }
}
