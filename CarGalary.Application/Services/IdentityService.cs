
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;

namespace CarGalary.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IdentityService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task AssignRoleAsync(string userId, string roleName)
        {
          await  _unitOfWork.identities.AssignRoleAsync(userId,roleName);
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            await _unitOfWork.identities.ChangePasswordAsync(userId,currentPassword,newPassword);
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
        return await _unitOfWork.identities.CheckPasswordAsync(userName,password);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await _unitOfWork.identities.CreateRoleAsync(roleName);
        }

        public async Task<string> CreateUserAsync(string userName, string email, string password)
        {
            return await _unitOfWork.identities.CreateUserAsync(userName, email,password);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await  _unitOfWork.identities.DeleteUserAsync(userId);
        }

        public async Task<string> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.identities.GetUserByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            return await  _unitOfWork.identities.GetUserRolesAsync(userId);
        }

        public async Task LockUserAsync(string userId)
        {
           await  _unitOfWork.identities.LockUserAsync(userId);
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
          return await  _unitOfWork.identities.LoginAsync(userName,password);
        }

        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            await  _unitOfWork.identities.RemoveRoleAsync(userId,roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await  _unitOfWork.identities.RoleExistsAsync(roleName);
        }

        public async Task UnlockUserAsync(string userId)
        {
           await  _unitOfWork.identities.UnlockUserAsync(userId);
        }

        public async Task UpdateEmailAsync(string userId, string newEmail)
        {
            await _unitOfWork.identities.UpdateEmailAsync(userId,newEmail);
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            await _unitOfWork.identities.UpdateUsernameAsync(userId,newUsername);
        }
    }
}