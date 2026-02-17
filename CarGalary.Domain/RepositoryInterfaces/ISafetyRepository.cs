using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface ISafetyRepository
    {
        Task<IEnumerable<Safety>> GetAllAsync();
        Task<Safety> GetByIdAsync(int id);
        Task CreateAsync(Safety model);
        Task UpdateAsync(Safety model);
        Task DeleteAsync(Safety model);
    }
}
