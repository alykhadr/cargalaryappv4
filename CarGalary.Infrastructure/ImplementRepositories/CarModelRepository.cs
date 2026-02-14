using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly ApplicationDbContext _context;

        public CarModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarModel>> GetAllAsync()
        {
            return await _context.CarModels.Where(c=>c.IsAvailable)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CarModel?> GetByIdAsync(int id)
        {
            var models= await GetAllAsync();
            return models.FirstOrDefault(x => x.Id == id);
        }

        public Task CreateAsync(CarModel model)
        {
            _context.CarModels.Add(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CarModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CarModel model)
        {
            model.IsAvailable=false;
            _context.Entry(model).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}

