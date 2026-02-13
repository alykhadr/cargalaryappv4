
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CarGalary.Infrastructure.ImplementRepositories
{

    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetProfileByUserIdAsync(int userId)
        {
            // var profile = await _context.Profiles
            //     .Include(up => up.User)
            //     .FirstOrDefaultAsync(up => up.UserId == userId);

            return null;
        }

        public async Task CreateProfileAsync(UserProfile userProfile)
        {

            _context.Profiles.Add(userProfile);

        }

        public async Task UpdateProfileAsync(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Modified;

        }

        public async Task DeleteProfileAsync(UserProfile userProfile)
        {

            _context.Profiles.Remove(userProfile);
        }
    }
}