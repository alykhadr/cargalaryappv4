using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class SafetyRepository : ISafetyRepository
    {
        private readonly ApplicationDbContext _context;

        public SafetyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Safety>> GetAllAsync()
        {
            return await _context.Safeties.ToListAsync();
        }

        public async Task<Safety> GetByIdAsync(int id)
        {
            return await _context.Safeties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Safety model)
        {
            _context.Safeties.Add(model);
        }

        public async Task UpdateAsync(Safety model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Safety model)
        {
            _context.Safeties.Remove(model);
        }
    }
}
