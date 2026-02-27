using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByUserIdAsync(Guid userId);
        Task<List<Employee>> GetAllWithDetailsAsync();
        Task<List<Employee>> GetByBranchIdWithDetailsAsync(int branchId);
        Task DeleteAsync(Employee employee);
    }
}
