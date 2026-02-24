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

        public async Task<(ApplicationUser User, string Token)> CreateUserAsync(string userName, string email, string password, string? firstName, string? lastName, int branchId, string? profileImageUrl)
        {
            var normalizedUserName = userName.Trim().ToUpperInvariant();
            var userNameExists = await _userManager.Users.AnyAsync(u => u.NormalizedUserName == normalizedUserName);
            if (userNameExists)
            {
                throw new Exception($"Username '{userName.Trim()}' already exists");
            }

            var user = new ApplicationUser
            {
                UserName = userName.Trim(),
                Email = email.Trim(),
                FullNameEn = firstName,
                FullNameAr = lastName,
                BranchId = branchId,
                ProfileImageUrl = profileImageUrl
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

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersByBranchAsync(int branchId)
        {
            return await _userManager.Users
                .Where(u => u.BranchId == branchId)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateUserDetailsAsync(string userId, string userName, string email, string? firstName, string? lastName, int branchId, string? profileImageUrl)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var normalizedUserName = userName.Trim().ToUpperInvariant();
            var normalizedEmail = email.Trim().ToUpperInvariant();

            var userNameExists = await _userManager.Users
                .AnyAsync(u => u.Id != user.Id && u.NormalizedUserName == normalizedUserName);
            if (userNameExists)
            {
                throw new Exception($"Username '{userName.Trim()}' already exists");
            }

            var emailExists = await _userManager.Users
                .AnyAsync(u => u.Id != user.Id && u.NormalizedEmail == normalizedEmail);
            if (emailExists)
            {
                throw new Exception($"Email '{email.Trim()}' already exists");
            }

            user.UserName = userName.Trim();
            user.Email = email.Trim();
            user.FullNameEn = firstName?.Trim();
            user.FullNameAr = lastName?.Trim();
            user.BranchId = branchId;
            if (profileImageUrl != null)
            {
                user.ProfileImageUrl = profileImageUrl;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task ChangeUserPasswordByAdminAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IList<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    continue;
                }

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims.Where(c => c.Type == "permission" && !string.IsNullOrWhiteSpace(c.Value)))
                {
                    permissions.Add(claim.Value.Trim());
                }
            }

            return permissions.OrderBy(p => p).ToList();
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

        public async Task<List<ApplicationUser>> GetUsersByRoleIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                return new List<ApplicationUser>();
            }

            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            return users
                .OrderByDescending(user => user.CreatedAt)
                .ToList();
        }

        public async Task<IList<string>> GetPermissionsAsync()
        {
            var roleIds = await _roleManager.Roles
                .Select(r => r.Id)
                .ToListAsync();

            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var roleId in roleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role == null)
                {
                    continue;
                }

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims.Where(c => c.Type == "permission" && !string.IsNullOrWhiteSpace(c.Value)))
                {
                    permissions.Add(claim.Value.Trim());
                }
            }

            return permissions.OrderBy(p => p).ToList();
        }

        public async Task<IList<string>> GetRolePermissionsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims
                .Where(c => c.Type == "permission" && !string.IsNullOrWhiteSpace(c.Value))
                .Select(c => c.Value.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c)
                .ToList();
        }

        public async Task AssignPermissionToRoleAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var normalizedPermission = permission.Trim();
            if (string.IsNullOrWhiteSpace(normalizedPermission))
            {
                throw new Exception("Permission is required");
            }

            var parts = normalizedPermission.Split(".", 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new Exception("Permission must be in page.action format");
            }

            normalizedPermission = string.Concat(parts[0], ".", parts[1]);

            var claims = await _roleManager.GetClaimsAsync(role);
            if (claims.Any(c => c.Type == "permission" && string.Equals(c.Value, normalizedPermission, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var result = await _roleManager.AddClaimAsync(role, new Claim("permission", normalizedPermission));
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task RemovePermissionFromRoleAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var toRemove = claims
                .Where(c => c.Type == "permission" && string.Equals(c.Value, permission, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var claim in toRemove)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
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

        public async Task<(ApplicationUser User, string Token)> LoginAsync(string userName, string password, bool rememberMe = false)
        {
            var user = await FindByUserNameOrEmailAsync(userName);
            var token = await LoginInternalAsync(user, password, rememberMe);
            return (user, token);
        }

        private async Task<ApplicationUser> FindByUserNameOrEmailAsync(string userNameOrEmail)
        {
            var normalized = userNameOrEmail.Trim();
            var normalizedUpper = normalized.ToUpperInvariant();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUpper);
            if (user != null)
            {
                return user;
            }

            user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedUpper);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid user name or password");
            }

            return user;
        }

        private async Task<string> LoginInternalAsync(ApplicationUser user, string password, bool rememberMe = false)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Invalid user name or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    continue;
                }

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims.Where(c => c.Type == "permission" && !string.IsNullOrWhiteSpace(c.Value)))
                {
                    permissions.Add(claim.Value.Trim());
                }
            }

            return GenerateJwt(user, roles, permissions.ToList(), rememberMe);
        }

        private string GenerateJwt(ApplicationUser user, IList<string> roles, IList<string> permissions, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
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
