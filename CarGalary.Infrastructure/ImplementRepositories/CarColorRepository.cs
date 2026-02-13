
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

        public async Task AddCarColorAsync(CarColor carColor)
        {
           
            _context.CarColors.Add(carColor);
        }

        public async Task<CarColor> GetCarColorByIdAsync(int id)
        {
            return await _context.CarColors.FindAsync(id);
        }

        public async Task<IEnumerable<CarColor>> GetAllCarColorsAsync()
        {
            return await _context.CarColors.ToListAsync();
        }

        public async Task UpdateCarColorAsync(CarColor carColor)
        {

            _context.Entry(carColor).State = EntityState.Modified;

        }

        public async Task DeleteCarColorAsync(CarColor carColor)
        {
            _context.CarColors.Remove(carColor);

        }
    }
}