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
    }
}
