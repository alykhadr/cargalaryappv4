



using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
   public interface IFavoritesRepository
{
    Task<IEnumerable<UserFavorite>> GetAllAsync();
    Task<UserFavorite> GetByIdAsync(Guid userId, int carId);
    Task CreateAsync(UserFavorite userFavorite);
    Task UpdateAsync(UserFavorite userFavorite);
    Task DeleteAsync(UserFavorite userFavorite);

    Task AddToFavoritesAsync(UserFavorite userFavorite);
    Task RemoveFromFavoritesAsync(UserFavorite userFavorite);
    Task<List<UserFavorite>> GetMyFavoritesAsync(Guid userId);
}
}
