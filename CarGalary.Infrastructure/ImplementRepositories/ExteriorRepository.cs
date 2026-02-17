using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class ExteriorRepository : IExteriorRepository
    {
        private readonly ApplicationDbContext _context;

        public ExteriorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exterior>> GetAllAsync()
        {
            return await _context.Exteriors.ToListAsync();
        }

        public async Task<Exterior> GetByIdAsync(int id)
        {
            return await _context.Exteriors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Exterior model)
        {
            _context.Exteriors.Add(model);
        }

        public async Task UpdateAsync(Exterior model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Exterior model)
        {
            _context.Exteriors.Remove(model);
        }
    }
}
