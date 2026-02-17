using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class MeasurementsRepository : IMeasurementsRepository
    {
        private readonly ApplicationDbContext _context;

        public MeasurementsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Measurements>> GetAllAsync()
        {
            return await _context.Measurements.ToListAsync();
        }

        public async Task<Measurements> GetByIdAsync(int id)
        {
            return await _context.Measurements.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Measurements model)
        {
            _context.Measurements.Add(model);
        }

        public async Task UpdateAsync(Measurements model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Measurements model)
        {
            _context.Measurements.Remove(model);
        }
    }
}
