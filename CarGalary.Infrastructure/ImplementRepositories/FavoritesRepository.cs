

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

        public async Task AddToFavoritesAsync(UserFavorite userFavorite)
        {
            

            _context.UserFavorites.Add(userFavorite);

        }

        public async Task RemoveFromFavoritesAsync(UserFavorite userFavorite)
        {
        
            _context.UserFavorites.Remove(userFavorite);

        }
        public async Task<List<UserFavorite>> GetMyFavoritesAsync(int userId)
        {
            // return await _context.UserFavorites
            //     .Where(f => f.UserId == userId)
                
            //     .ToListAsync();

            return null;
        }

       

       
    }
}