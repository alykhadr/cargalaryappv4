using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IPrivacyPolicyRepository
    {
        Task<IEnumerable<PrivacyPolicy>> GetAllAsync();
        Task<IEnumerable<PrivacyPolicy>> GetAvailableAsync();
        Task<PrivacyPolicy?> GetByIdAsync(int id);
        Task<PrivacyPolicy?> GetLatestAvailableAsync();
        Task CreateAsync(PrivacyPolicy privacyPolicy);
        Task UpdateAsync(PrivacyPolicy privacyPolicy);
        Task DeleteAsync(PrivacyPolicy privacyPolicy);
    }
}
