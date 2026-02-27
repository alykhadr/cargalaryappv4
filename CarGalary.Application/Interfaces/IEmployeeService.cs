using CarGalary.Application.Dtos.Auth;

namespace CarGalary.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task CreateEmployeeAsync(RegisterRequest request, Guid userId);
        Task<List<UserListItemDto>> GetEmployeesAsync();
        Task<List<UserListItemDto>> GetEmployeesByBranchAsync(int branchId);
        Task UpdateEmployeeAsync(Guid userId, UpdateAdminUserRequest request);
        Task DeleteEmployeeAsync(Guid userId);
    }
}
