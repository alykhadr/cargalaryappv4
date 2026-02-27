using CarGalary.Domain.Entities;

namespace CarGalary.Domain.RepositoryInterfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<Department?> GetByNameArAsync(string nameAr);
        Task<Department?> GetByNameEnAsync(string nameEn);
        Task CreateAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Department department);
    }
}
