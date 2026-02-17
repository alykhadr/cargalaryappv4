

using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    // this service to add car to favorite list by client 
    public class FavoritesRepository: IFavoritesRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoritesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserFavorite>> GetAllAsync()
        {
            return await _context.UserFavorites.ToListAsync();
        }

        public async Task<UserFavorite> GetByIdAsync(Guid userId, int carId)
        {
            return await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId && x.CarId == carId);
        }

        public async Task CreateAsync(UserFavorite userFavorite)
        {
            _context.UserFavorites.Add(userFavorite);
        }

        public async Task UpdateAsync(UserFavorite userFavorite)
        {
            _context.Entry(userFavorite).State = EntityState.Modified;
        }

        public async Task DeleteAsync(UserFavorite userFavorite)
        {
            _context.UserFavorites.Remove(userFavorite);
        }

        public async Task AddToFavoritesAsync(UserFavorite userFavorite)
        {
            

            _context.UserFavorites.Add(userFavorite);

        }

        public async Task RemoveFromFavoritesAsync(UserFavorite userFavorite)
        {
        
            _context.UserFavorites.Remove(userFavorite);

        }
        public async Task<List<UserFavorite>> GetMyFavoritesAsync(Guid userId)
        {
            return await _context.UserFavorites
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

       

       
    }
}
