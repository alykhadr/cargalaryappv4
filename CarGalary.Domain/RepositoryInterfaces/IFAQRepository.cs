


using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IFAQRepository
{
    Task<IEnumerable<FAQ>> GetAllAsync();
    Task<IEnumerable<FAQ>> GetAvailableAsync();
    Task<FAQ> GetByIdAsync(int id);
    Task CreateAsync(FAQ faq);
    Task UpdateAsync(FAQ faq);
    Task DeleteAsync(FAQ  fAQ);
}

}