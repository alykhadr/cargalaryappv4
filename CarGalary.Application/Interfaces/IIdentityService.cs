using CarGalary.Application.Dtos.Auth;

namespace CarGalary.Application.Interfaces
{
    public interface IIdentityService
    {
        // User
        Task<UserDto> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<List<UserListItemDto>> GetUsersAsync();
        Task UpdateUserDetailsAsync(string userId, string userName, string email, string? firstName, string? lastName);
        Task ChangeUserPasswordByAdminAsync(string userId, string newPassword);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IList<string>> GetUserPermissionsAsync(string userId);
        Task LockUserAsync(string userId);
        Task UnlockUserAsync(string userId);
        Task<string> GetUserByEmailAsync(string email);

        // Role
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(string roleId);
        Task<List<RoleUserDto>> GetUsersByRoleIdAsync(string roleId);
        Task<IList<string>> GetPermissionsAsync();
        Task<IList<string>> GetRolePermissionsAsync(string roleId);
        Task AssignPermissionToRoleAsync(string roleId, string permission);
        Task RemovePermissionFromRoleAsync(string roleId, string permission);
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
