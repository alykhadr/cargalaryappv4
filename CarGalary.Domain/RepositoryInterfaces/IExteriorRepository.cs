using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IExteriorRepository
    {
        Task<IEnumerable<Exterior>> GetAllAsync();
        Task<Exterior> GetByIdAsync(int id);
        Task CreateAsync(Exterior model);
        Task UpdateAsync(Exterior model);
        Task DeleteAsync(Exterior model);
    }
}
