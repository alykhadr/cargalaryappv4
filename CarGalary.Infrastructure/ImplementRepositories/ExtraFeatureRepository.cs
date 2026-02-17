using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class ExtraFeatureRepository : IExtraFeatureRepository
    {
        private readonly ApplicationDbContext _context;

        public ExtraFeatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExtraFeature>> GetAllAsync()
        {
            return await _context.ExtraFeatures.ToListAsync();
        }

        public async Task<ExtraFeature> GetByIdAsync(int id)
        {
            return await _context.ExtraFeatures.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(ExtraFeature model)
        {
            _context.ExtraFeatures.Add(model);
        }

        public async Task UpdateAsync(ExtraFeature model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(ExtraFeature model)
        {
            _context.ExtraFeatures.Remove(model);
        }
    }
}
