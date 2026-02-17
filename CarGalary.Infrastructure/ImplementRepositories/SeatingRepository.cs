using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class SeatingRepository : ISeatingRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seating>> GetAllAsync()
        {
            return await _context.Seatings.ToListAsync();
        }

        public async Task<Seating> GetByIdAsync(int id)
        {
            return await _context.Seatings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Seating model)
        {
            _context.Seatings.Add(model);
        }

        public async Task UpdateAsync(Seating model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Seating model)
        {
            _context.Seatings.Remove(model);
        }
    }
}
