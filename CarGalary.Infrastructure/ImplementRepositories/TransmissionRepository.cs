using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class TransmissionRepository : ITransmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transmission>> GetAllAsync()
        {
            return await _context.Transmissions.ToListAsync();
        }

        public async Task<Transmission> GetByIdAsync(int id)
        {
            return await _context.Transmissions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Transmission model)
        {
            _context.Transmissions.Add(model);
        }

        public async Task UpdateAsync(Transmission model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Transmission model)
        {
            _context.Transmissions.Remove(model);
        }
    }
}
