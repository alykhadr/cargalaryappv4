using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IExtraFeatureRepository
    {
        Task<IEnumerable<ExtraFeature>> GetAllAsync();
        Task<ExtraFeature> GetByIdAsync(int id);
        Task CreateAsync(ExtraFeature model);
        Task UpdateAsync(ExtraFeature model);
        Task DeleteAsync(ExtraFeature model);
    }
}
