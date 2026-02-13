


using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.Repositories
{

    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CarBrand> BrandExists(int id)
        {
            var carbrand = await _context.CarBrands.FirstOrDefaultAsync(e => e.Id == id);
            return carbrand;
        }

        public async Task CreateBrand(CarBrand brand)
        {
            _context.CarBrands.Add(brand);
            
        }

        public async Task DeleteBrandById(CarBrand carBrand)
        {
            _context.CarBrands.Remove(carBrand);
           
        }

        public async Task<CarBrand> GetBrandById(int id)
        {
            return await _context.CarBrands
                                 .Include(b => b.CarModels)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CarBrand>> GetBrands()
        {
            return await _context.CarBrands
                                  .Include(b => b.CarModels) // Include related cars
                                  .ToListAsync();
        }

        public async Task UpdateBrand(CarBrand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;
            
        }
    }
}