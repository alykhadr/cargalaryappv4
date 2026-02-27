using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.OrderByDescending(d => d.CreatedAt).ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department?> GetByNameArAsync(string nameAr)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.NameAr == nameAr);
        }

        public async Task<Department?> GetByNameEnAsync(string nameEn)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.NameEn == nameEn);
        }

        public async Task CreateAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
        }
    }
}
