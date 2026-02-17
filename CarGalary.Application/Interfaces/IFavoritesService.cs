using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;

namespace CarGalary.Application.Interfaces
{
   public interface IFavoritesService
   {
      Task<List<UserFavoriteAdminResponseDto>> GetAllAsync();
      Task<UserFavoriteAdminResponseDto?> GetByIdAsync(Guid userId, int carId);
      Task<UserFavoriteAdminResponseDto> CreateAsync(CreateUserFavoriteAdminRequestDto dto);
      Task UpdateAsync(Guid userId, int carId, UpdateUserFavoriteAdminRequestDto dto);
      Task DeleteAsync(Guid userId, int carId);
   }
}
