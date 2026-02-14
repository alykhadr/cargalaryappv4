using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarCarColorRepository : ICarCarColorRepository
    {
        private readonly ApplicationDbContext _context;

        public CarCarColorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarCarColor>> GetAllAsync()
        {
            return await _context.CarCarColors
                .Where(x => x.IsAvailable)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CarCarColor?> GetByIdAsync(int carId, int colorId)
        {
            var entity = await _context.CarCarColors.FindAsync(carId, colorId);
            return entity != null && entity.IsAvailable ? entity : null;
        }

        public async Task<List<CarCarColor>> GetByCarIdAsync(int carId)
        {
            return await _context.CarCarColors
                .Where(x => x.IsAvailable && x.CarId == carId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task CreateAsync(CarCarColor entity)
        {
            _context.CarCarColors.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CarCarColor entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CarCarColor entity)
        {
            entity.IsAvailable = false;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}

