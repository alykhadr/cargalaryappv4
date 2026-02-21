using CarGalary.Application.Dtos.Auth;

namespace CarGalary.Application.Interfaces
{
    public interface IIdentityService
    {
        // User
        Task<UserDto> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task LockUserAsync(string userId);
        Task UnlockUserAsync(string userId);
        Task<string> GetUserByEmailAsync(string email);

        // Role
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(string roleId);
        Task<RoleDto> CreateRoleAsync(CreateRoleRequest request);
        Task<bool> UpdateRoleAsync(string roleId, UpdateRoleRequest request);
        Task<bool> DeleteRoleAsync(string roleId);

        Task CreateRoleAsync(string roleName);
        Task AssignRoleAsync(string userId, string roleName);
        Task RemoveRoleAsync(string userId, string roleName);
        Task<bool> RoleExistsAsync(string roleName);

        // Auth
        Task<UserDto> LoginAsync(string userName, string password);

        // =================== PROFILE MANAGEMENT ===================
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task UpdateEmailAsync(string userId, string newEmail);
        Task UpdateUsernameAsync(string userId, string newUsername);
    }
}
