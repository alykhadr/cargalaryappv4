using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarExtraDetailsRepository : ICarExtraDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public CarExtraDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarExtraDetails>> GetAllAsync()
        {
            return await _context.CarExtraDetails
                .Where(x => x.IsAvailable)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CarExtraDetails?> GetByIdAsync(int id)
        {
            // Keep behavior consistent with other repos: filter by IsAvailable.
            var all = await GetAllAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<CarExtraDetails>> GetByCarIdAsync(int carId)
        {
            return await _context.CarExtraDetails
                .Where(x => x.IsAvailable && x.CarId == carId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task CreateAsync(CarExtraDetails entity)
        {
            _context.CarExtraDetails.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CarExtraDetails entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CarExtraDetails entity)
        {
            entity.IsAvailable = false;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}

