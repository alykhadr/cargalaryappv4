using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IPackagesRepository
    {
        Task<IEnumerable<Packages>> GetAllAsync();
        Task<Packages?> GetByIdAsync(int id);
        Task CreateAsync(Packages package);
        Task UpdateAsync(Packages package);
        Task DeleteAsync(Packages package);
    }
}
