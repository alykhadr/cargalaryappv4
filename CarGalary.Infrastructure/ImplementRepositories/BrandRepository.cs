


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

        public async Task<Brand?> BrandExists(int id)
        {
            var carbrand = await _context.Brands.FirstOrDefaultAsync(e => e.Id == id);
            return carbrand;
        }

        public Task CreateBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            return Task.CompletedTask;
        }

        public Task DeleteBrandById(Brand carBrand)
        {
            carBrand.IsAvailable=false;
            _context.Entry(carBrand).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<Brand?> GetBrandById(int id)
        {
            var brands = await GetBrands();
            return brands.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Brand>> GetBrands()
        {
            return await _context.Brands.Where(c => c.IsAvailable)
                                  .Include(b => b.CarModels) // Include related cars
                                  .ToListAsync();
        }

        public Task UpdateBrand(Brand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
