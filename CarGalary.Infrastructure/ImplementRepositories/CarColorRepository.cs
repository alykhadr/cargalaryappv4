
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarColorRepository : ICarColorRepository
    {
        private readonly ApplicationDbContext _context;

        public CarColorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCarColorAsync(Color carColor)
        {
           
            _context.Colors.Add(carColor);
        }

        public async Task<Color> GetCarColorByIdAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public async Task<IEnumerable<Color>> GetAllCarColorsAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        public async Task UpdateCarColorAsync(Color carColor)
        {

            _context.Entry(carColor).State = EntityState.Modified;

        }

        public async Task DeleteCarColorAsync(Color carColor)
        {
            _context.Colors.Remove(carColor);

        }
    }
}