

using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IContactSalesOfficerRepository
    {
        Task<IEnumerable<ContactSalesOfficer>> GetAllAsync();
    Task<ContactSalesOfficer> GetByIdAsync(int id);
    Task CreateAsync(ContactSalesOfficer model);
    Task UpdateAsync(ContactSalesOfficer model);
    Task DeleteAsync(ContactSalesOfficer contactSalesOfficer);
    }
}