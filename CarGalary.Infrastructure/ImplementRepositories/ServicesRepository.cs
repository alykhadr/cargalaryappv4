
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
   public class ServicesRepository : IServicesRepository
{
    private readonly ApplicationDbContext _context;

    public ServicesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Services>> GetAllAsync()
    {
        return await _context.Services
            .Include(x => x.ServiceType)
            .ToListAsync();
    }

    public async Task<Services> GetByIdAsync(int id)
    {
        return await _context.Services
            .Include(x => x.ServiceType)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Services>> GetByServiceTypeIdAsync(int serviceTypeId)
    {
        return await _context.Services
            .Where(x => x.ServiceTypeId == serviceTypeId)
            .ToListAsync();
    }

    public async Task CreateAsync(Services service)
    {
        _context.Services.Add(service);
    }

    public async Task UpdateAsync(Services service)
    {
         _context.Entry(service).State = EntityState.Modified;

    }

    public async Task DeleteAsync(Services service)
    {
        _context.Services.Remove(service);

    }
}
}