using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IPrivacyPolicyRepository
    {
        Task<IEnumerable<PrivacyPolicy>> GetAllAsync();
        Task<PrivacyPolicy?> GetFirstAsync();
        Task<PrivacyPolicy?> GetByIdAsync(int id);
        Task CreateAsync(PrivacyPolicy privacyPolicy);
        Task UpdateAsync(PrivacyPolicy privacyPolicy);
        Task DeleteAsync(PrivacyPolicy privacyPolicy);
    }
}
