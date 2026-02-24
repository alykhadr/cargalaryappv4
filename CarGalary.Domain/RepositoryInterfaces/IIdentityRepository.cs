using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IIdentityRepository
    {
        // User
        Task<(ApplicationUser User, string Token)> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName, int branchId, string? profileImageUrl);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<List<ApplicationUser>> GetUsersByBranchAsync(int branchId);
        Task UpdateUserDetailsAsync(string userId, string userName, string email, string? firstName, string? lastName, int branchId, string? profileImageUrl);
        Task ChangeUserPasswordByAdminAsync(string userId, string newPassword);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IList<string>> GetUserPermissionsAsync(string userId);
        Task LockUserAsync(string userId);
        Task UnlockUserAsync(string userId);
        Task<string> GetUserByEmailAsync(string email);

        // Role
        Task<List<ApplicationRole>> GetRolesAsync();
        Task<ApplicationRole?> GetRoleByIdAsync(string roleId);
        Task<List<ApplicationUser>> GetUsersByRoleIdAsync(string roleId);
        Task<IList<string>> GetPermissionsAsync();
        Task<IList<string>> GetRolePermissionsAsync(string roleId);
        Task AssignPermissionToRoleAsync(string roleId, string permission);
        Task RemovePermissionFromRoleAsync(string roleId, string permission);
        Task<ApplicationRole> CreateRoleAsync(string roleName, bool isActive);
        Task<bool> UpdateRoleAsync(string roleId, string roleName, bool isActive);
        Task<bool> DeleteRoleAsync(string roleId);

        Task CreateRoleAsync(string roleName);
        Task AssignRoleAsync(string userId, string roleName);
        Task RemoveRoleAsync(string userId, string roleName);
        Task<bool> RoleExistsAsync(string roleName);

        // Auth
        Task<(ApplicationUser User, string Token)> LoginAsync(string userName, string password, bool rememberMe = false);

        // =================== PROFILE MANAGEMENT ===================
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task UpdateEmailAsync(string userId, string newEmail);
        Task UpdateUsernameAsync(string userId, string newUsername);
    }
}
