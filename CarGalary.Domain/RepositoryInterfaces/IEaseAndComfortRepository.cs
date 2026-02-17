using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IEaseAndComfortRepository
    {
        Task<IEnumerable<EaseAndComfort>> GetAllAsync();
        Task<EaseAndComfort> GetByIdAsync(int id);
        Task CreateAsync(EaseAndComfort model);
        Task UpdateAsync(EaseAndComfort model);
        Task DeleteAsync(EaseAndComfort model);
    }
}
