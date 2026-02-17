using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class EngineSpecificationRepository : IEngineSpecificationRepository
    {
        private readonly ApplicationDbContext _context;

        public EngineSpecificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EngineSpecification>> GetAllAsync()
        {
            return await _context.EngineSpecifications.ToListAsync();
        }

        public async Task<EngineSpecification> GetByIdAsync(int id)
        {
            return await _context.EngineSpecifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(EngineSpecification model)
        {
            _context.EngineSpecifications.Add(model);
        }

        public async Task UpdateAsync(EngineSpecification model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(EngineSpecification model)
        {
            _context.EngineSpecifications.Remove(model);
        }
    }
}
