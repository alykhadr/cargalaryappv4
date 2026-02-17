using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ITransmissionRepository
    {
        Task<IEnumerable<Transmission>> GetAllAsync();
        Task<Transmission> GetByIdAsync(int id);
        Task CreateAsync(Transmission model);
        Task UpdateAsync(Transmission model);
        Task DeleteAsync(Transmission model);
    }
}
