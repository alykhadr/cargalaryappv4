using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarGalary.Infrastructure.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtSettings _jwt;

        public IdentityRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }


        // ================= USER =================

        public async Task<(ApplicationUser User, string Token)> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FullNameEn = firstName,
                FullNameAr = lastName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var token = await LoginInternalAsync(user, password);
            return (user, token);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            return await _userManager.GetRolesAsync(user);
        }

        public async Task LockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }

        public async Task UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.SetLockoutEndDateAsync(user, null);
        }

        // ================= ROLE =================

        public async Task<List<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<ApplicationRole> CreateRoleAsync(string roleName, bool isActive)
        {
            var normalizedRole = roleName.Trim();

            if (await _roleManager.RoleExistsAsync(normalizedRole))
                throw new Exception($"Role '{normalizedRole}' already exists");

            var role = new ApplicationRole
            {
                Name = normalizedRole,
                IsActive = isActive,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return role;
        }

        public async Task<bool> UpdateRoleAsync(string roleId, string roleName, bool isActive)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }

            var normalizedRole = roleName.Trim();
            var existingByName = await _roleManager.FindByNameAsync(normalizedRole);
            if (existingByName != null && existingByName.Id != role.Id)
                throw new Exception($"Role '{normalizedRole}' already exists");

            role.Name = normalizedRole;
            role.IsActive = isActive;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName, IsActive = true, CreatedAt = DateTime.UtcNow });
        }

        public async Task AssignRoleAsync(string userId, string roleName)
        {
            await CreateRoleAsync(roleName);

            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        // ================= AUTH =================

        public async Task<(ApplicationUser User, string Token)> LoginAsync(string userName, string password)
        {
            var user = await FindByUserNameOrEmailAsync(userName);
            var token = await LoginInternalAsync(user, password);
            return (user, token);
        }

        private async Task<ApplicationUser> FindByUserNameOrEmailAsync(string userNameOrEmail)
        {
            var normalized = userNameOrEmail.Trim();
            var user = await _userManager.FindByNameAsync(normalized);
            if (user != null)
            {
                return user;
            }

            user = await _userManager.FindByEmailAsync(normalized.ToUpper());
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid user name or password");
            }

            return user;
        }

        private async Task<string> LoginInternalAsync(ApplicationUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Invalid user name or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return GenerateJwt(user, roles);
        }

        private string GenerateJwt(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // =================== PROFILE MANAGEMENT ===================

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task UpdateEmailAsync(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.UserName = newUsername;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<string> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null ? user.Id.ToString() : "";
        }
    }
}
