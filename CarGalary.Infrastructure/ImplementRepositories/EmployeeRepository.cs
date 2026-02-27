using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task<Employee?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<List<Employee>> GetAllWithDetailsAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.User)
                .Include(e => e.Branch)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetByBranchIdWithDetailsAsync(int branchId)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.User)
                .Include(e => e.Branch)
                .Where(e => e.BranchId == branchId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
        }
    }
}
