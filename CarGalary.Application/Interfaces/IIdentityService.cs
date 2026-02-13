using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarGalary.Application.Interfaces
{
    public interface IIdentityService
    {
        // User
        Task<string> CreateUserAsync(string userName, string email, string password);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task LockUserAsync(string userId);
        Task UnlockUserAsync(string userId);

         Task<string> GetUserByEmailAsync(string email);

        // Role
        Task CreateRoleAsync(string roleName);
        Task AssignRoleAsync(string userId, string roleName);
        Task RemoveRoleAsync(string userId, string roleName);
        Task<bool> RoleExistsAsync(string roleName);

        // Auth
        Task<string> LoginAsync(string userName, string password);

        // =================== PROFILE MANAGEMENT ===================
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task UpdateEmailAsync(string userId, string newEmail);
        Task UpdateUsernameAsync(string userId, string newUsername);
    }

}