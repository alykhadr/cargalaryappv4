




using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IUserProfileRepository
    {
         Task<IEnumerable<UserProfile>> GetAllAsync();
         Task<UserProfile> GetByIdAsync(int id);
         Task<UserProfile> GetProfileByUserIdAsync(Guid userId);
         Task CreateProfileAsync(UserProfile userProfile);
        Task UpdateProfileAsync(UserProfile userProfile);
        Task DeleteProfileAsync(UserProfile userProfile);
    }
}
