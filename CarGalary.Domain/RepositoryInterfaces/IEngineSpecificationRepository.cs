using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IEngineSpecificationRepository
    {
        Task<IEnumerable<EngineSpecification>> GetAllAsync();
        Task<EngineSpecification> GetByIdAsync(int id);
        Task CreateAsync(EngineSpecification model);
        Task UpdateAsync(EngineSpecification model);
        Task DeleteAsync(EngineSpecification model);
    }
}
