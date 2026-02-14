


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

        public async Task<CarBrand?> BrandExists(int id)
        {
            var carbrand = await _context.CarBrands.FirstOrDefaultAsync(e => e.Id == id);
            return carbrand;
        }

        public Task CreateBrand(CarBrand brand)
        {
            _context.CarBrands.Add(brand);
            return Task.CompletedTask;
        }

        public Task DeleteBrandById(CarBrand carBrand)
        {
            carBrand.IsAvailable=false;
            _context.Entry(carBrand).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<CarBrand?> GetBrandById(int id)
        {
            var brands = await GetBrands();
            return brands.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<CarBrand>> GetBrands()
        {
            return await _context.CarBrands.Where(c => c.IsAvailable)
                                  .Include(b => b.CarModels) // Include related cars
                                  .ToListAsync();
        }

        public Task UpdateBrand(CarBrand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
