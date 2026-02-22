using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IdentityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await _unitOfWork.identities.GetRolesAsync();
            return roles.Select(ToRoleDto).ToList();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(string roleId)
        {
            var role = await _unitOfWork.identities.GetRoleByIdAsync(roleId);
            return role == null ? null : ToRoleDto(role);
        }

        public async Task<List<RoleUserDto>> GetUsersByRoleIdAsync(string roleId)
        {
            var users = await _unitOfWork.identities.GetUsersByRoleIdAsync(roleId);
            return users.Select(ToRoleUserDto).ToList();
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest request)
        {
            var role = await _unitOfWork.identities.CreateRoleAsync(request.Name, request.IsActive);
            return ToRoleDto(role);
        }

        public async Task<bool> UpdateRoleAsync(string roleId, UpdateRoleRequest request)
        {
            return await _unitOfWork.identities.UpdateRoleAsync(roleId, request.Name, request.IsActive);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            return await _unitOfWork.identities.DeleteRoleAsync(roleId);
        }

        public async Task AssignRoleAsync(string userId, string roleName)
        {
            await _unitOfWork.identities.AssignRoleAsync(userId, roleName);
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            await _unitOfWork.identities.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            return await _unitOfWork.identities.CheckPasswordAsync(userName, password);
        }

        public async Task<List<UserListItemDto>> GetUsersAsync()
        {
            var users = await _unitOfWork.identities.GetUsersAsync();
            return users.Select(ToUserListItemDto).ToList();
        }

        public async Task UpdateUserDetailsAsync(string userId, string userName, string email, string? firstName, string? lastName)
        {
            await _unitOfWork.identities.UpdateUserDetailsAsync(userId, userName, email, firstName, lastName);
        }

        public async Task ChangeUserPasswordByAdminAsync(string userId, string newPassword)
        {
            await _unitOfWork.identities.ChangeUserPasswordByAdminAsync(userId, newPassword);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await _unitOfWork.identities.CreateRoleAsync(roleName);
        }

        public async Task<UserDto> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName)
        {
            var result = await _unitOfWork.identities.CreateUserAsync(userName, email, password, firstName, lastName);
            return ToUserDto(result.User, result.Token);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _unitOfWork.identities.DeleteUserAsync(userId);
        }

        public async Task<string> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.identities.GetUserByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            return await _unitOfWork.identities.GetUserRolesAsync(userId);
        }

        public async Task<IList<string>> GetUserPermissionsAsync(string userId)
        {
            return await _unitOfWork.identities.GetUserPermissionsAsync(userId);
        }

        public async Task LockUserAsync(string userId)
        {
            await _unitOfWork.identities.LockUserAsync(userId);
        }

        public async Task<UserDto> LoginAsync(string userName, string password)
        {
            var result = await _unitOfWork.identities.LoginAsync(userName, password);
            return ToUserDto(result.User, result.Token);
        }

        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            await _unitOfWork.identities.RemoveRoleAsync(userId, roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _unitOfWork.identities.RoleExistsAsync(roleName);
        }

        public async Task<IList<string>> GetPermissionsAsync()
        {
            return await _unitOfWork.identities.GetPermissionsAsync();
        }

        public async Task<IList<string>> GetRolePermissionsAsync(string roleId)
        {
            return await _unitOfWork.identities.GetRolePermissionsAsync(roleId);
        }

        public async Task AssignPermissionToRoleAsync(string roleId, string permission)
        {
            await _unitOfWork.identities.AssignPermissionToRoleAsync(roleId, permission);
        }

        public async Task RemovePermissionFromRoleAsync(string roleId, string permission)
        {
            await _unitOfWork.identities.RemovePermissionFromRoleAsync(roleId, permission);
        }

        public async Task UnlockUserAsync(string userId)
        {
            await _unitOfWork.identities.UnlockUserAsync(userId);
        }

        public async Task UpdateEmailAsync(string userId, string newEmail)
        {
            await _unitOfWork.identities.UpdateEmailAsync(userId, newEmail);
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            await _unitOfWork.identities.UpdateUsernameAsync(userId, newUsername);
        }

        private static UserDto ToUserDto(ApplicationUser user, string token)
        {
            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.UserName,
                Password = null,
                FirstName = user.FullNameEn,
                LastName = user.FullNameAr,
                Token = token,
                Email = user.Email
            };
        }

        private static RoleDto ToRoleDto(ApplicationRole role)
        {
            return new RoleDto
            {
                Id = role.Id.ToString(),
                Name = role.Name ?? string.Empty,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt
            };
        }

        private static RoleUserDto ToRoleUserDto(ApplicationUser user)
        {
            return new RoleUserDto
            {
                Id = user.Id.ToString(),
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FullNameEn ?? string.Empty,
                LastName = user.FullNameAr ?? string.Empty
            };
        }

        private static UserListItemDto ToUserListItemDto(ApplicationUser user)
        {
            return new UserListItemDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FullNameEn ?? string.Empty,
                LastName = user.FullNameAr ?? string.Empty,
                CreatedAt = user.CreatedAt,
                IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow
            };
        }
    }
}
