using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class EaseAndComfortRepository : IEaseAndComfortRepository
    {
        private readonly ApplicationDbContext _context;

        public EaseAndComfortRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EaseAndComfort>> GetAllAsync()
        {
            return await _context.EaseAndComforts.ToListAsync();
        }

        public async Task<EaseAndComfort> GetByIdAsync(int id)
        {
            return await _context.EaseAndComforts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(EaseAndComfort model)
        {
            _context.EaseAndComforts.Add(model);
        }

        public async Task UpdateAsync(EaseAndComfort model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(EaseAndComfort model)
        {
            _context.EaseAndComforts.Remove(model);
        }
    }
}
