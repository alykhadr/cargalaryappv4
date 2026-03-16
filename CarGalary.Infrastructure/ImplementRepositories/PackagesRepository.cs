using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly ApplicationDbContext _context;

        public PackagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Packages>> GetAllAsync()
        {
            return await _context.Packages
                .Where(p => p.IsAvailable)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Packages?> GetByIdAsync(int id)
        {
            return await _context.Packages
                .FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);
        }

        public Task CreateAsync(Packages package)
        {
            _context.Packages.Add(package);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Packages package)
        {
            _context.Entry(package).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Packages package)
        {
            package.IsAvailable = false;
            _context.Entry(package).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
