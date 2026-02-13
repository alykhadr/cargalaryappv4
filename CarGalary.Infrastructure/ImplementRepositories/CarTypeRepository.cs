

using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarTypeRepository : ICarTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public CarTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CarType> CarTypeExists(int id)
        {
            var carType = await _context.CarTypes.FirstOrDefaultAsync(e => e.Id == id);
            return carType;
        }

        public async Task CreateCarType(CarType type)
        {
            _context.CarTypes.Add(type);
        }

        public async Task DeleteCarTypeById(CarType carType)
        {


            _context.CarTypes.Remove(carType);

        }

        public async Task<CarType> GetCarTypeById(int id)
        {
            return await _context.CarTypes
                                 .Include(b => b.Cars)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CarType>> GetCarTypes()
        {
            return await _context.CarTypes
                                 .Include(b => b.Cars) // Include related cars
                                 .ToListAsync();
        }

        public async Task UpdateCarType(CarType type)
        {


            _context.Entry(type).State = EntityState.Modified;

        }

       
    }
}