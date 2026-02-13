
using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{


public class FAQRepository : IFAQRepository
{
    private readonly ApplicationDbContext _context;

    public FAQRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FAQ>> GetAllAsync()
    {
        return await _context.FAQs
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<FAQ>> GetAvailableAsync()
    {
        return await _context.FAQs
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<FAQ> GetByIdAsync(int id)
    {
        return await _context.FAQs.FindAsync(id);
    }

    public async Task CreateAsync(FAQ faq)
    {
        _context.FAQs.Add(faq);
    }

    public async Task UpdateAsync( FAQ faq)
    {
          _context.Entry(faq).State = EntityState.Modified;

       
    }

    public async Task DeleteAsync(FAQ fAQ)
    {
    
        _context.FAQs.Remove(fAQ);

        
    }
}

}