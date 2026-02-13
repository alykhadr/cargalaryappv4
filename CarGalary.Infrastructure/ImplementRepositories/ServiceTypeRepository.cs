
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceType>> GetAllAsync()
        {
            return await _context.ServiceTypes.ToListAsync();
        }

        public async Task<ServiceType> GetByIdAsync(int id)
        {
            return await _context.ServiceTypes.FindAsync(id);
        }

        public async Task CreateAsync(ServiceType serviceType)
        {
            _context.ServiceTypes.Add(serviceType);
        }

        public async Task UpdateAsync(ServiceType serviceType)
        {
            _context.Entry(serviceType).State = EntityState.Modified;

        }

        public async Task DeleteAsync(ServiceType serviceType)
        {


            _context.ServiceTypes.Remove(serviceType);

        }
    }
}