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

        public async Task<List<CarColor>> GetAllAsync()
        {
            return await _context.CarColors
                .Where(x => x.IsAvailable)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CarColor?> GetByIdAsync(int carId, int colorId)
        {
            var entity = await _context.CarColors.FindAsync(carId, colorId);
            return entity != null && entity.IsAvailable ? entity : null;
        }

        public async Task<List<CarColor>> GetByCarIdAsync(int carId)
        {
            return await _context.CarColors
                .Where(x => x.IsAvailable && x.CarId == carId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task CreateAsync(CarColor entity)
        {
            _context.CarColors.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CarColor entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CarColor entity)
        {
            entity.IsAvailable = false;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}

