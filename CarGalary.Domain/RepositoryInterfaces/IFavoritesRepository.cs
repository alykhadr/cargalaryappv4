



using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
   public interface IFavoritesRepository
{
    Task AddToFavoritesAsync(UserFavorite userFavorite);
    Task RemoveFromFavoritesAsync(UserFavorite userFavorite);
    //TODO
    Task<List<UserFavorite>> GetMyFavoritesAsync(int userId);
}
}