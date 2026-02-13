



using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
   public interface IServicesRepository
{
    Task<IEnumerable<Services>> GetAllAsync();
    Task<Services> GetByIdAsync(int id);
    Task<IEnumerable<Services>> GetByServiceTypeIdAsync(int serviceTypeId);
    Task CreateAsync(Services service);
    Task UpdateAsync(Services service);
    Task DeleteAsync(Services services);
}
}