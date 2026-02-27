
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarRepository :ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars
                .Include(c => c.CarModel)
                .Include(c => c.Type)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<Car> GetByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.CarModel)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Car car)
        {
            _context.Cars.Add(car);
        }

        public async Task UpdateAsync(Car car)
        {
           

            _context.Entry(car).State = EntityState.Modified;

        }

        public async Task DeleteAsync(Car car)
        {
            _context.Cars.Remove(car);
           
        }

        public async Task<Car> CarExistsAsync(int id)
        {
            var car= await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
        return car;
        }

        public async Task<List<Car>> FilterAsync(int? modelId = null, int? typeId = null, bool? isAvailable = null)
        {
            var query = _context.Cars
                .Include(c => c.CarModel)
                .Include(c => c.Type)
                .AsQueryable();

            if (modelId.HasValue) query = query.Where(c => c.ModelId == modelId.Value);
            if (typeId.HasValue) query = query.Where(c => c.TypeId == typeId.Value);
            if (isAvailable.HasValue) query = query.Where(c => c.IsAvailable == isAvailable.Value);

            return await query.ToListAsync();
        }
    }
}
