




using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IUserProfileRepository
    {
       //TODO
         Task<UserProfile> GetProfileByUserIdAsync(int userId);
         Task CreateProfileAsync(UserProfile userProfile);
        Task UpdateProfileAsync(UserProfile userProfile);
        Task DeleteProfileAsync(UserProfile userProfile);
    }
}