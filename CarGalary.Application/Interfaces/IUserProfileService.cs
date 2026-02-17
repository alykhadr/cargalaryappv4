using CarGalary.Application.Dtos.UserProfileAdmin.Command;
using CarGalary.Application.Dtos.UserProfileAdmin.Query;

namespace CarGalary.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<List<UserProfileAdminResponseDto>> GetAllAsync();
        Task<UserProfileAdminResponseDto?> GetByIdAsync(int id);
        Task<UserProfileAdminResponseDto> CreateAsync(CreateUserProfileAdminRequestDto dto);
        Task UpdateAsync(int id, UpdateUserProfileAdminRequestDto dto);
        Task DeleteAsync(int id);
    }
}
