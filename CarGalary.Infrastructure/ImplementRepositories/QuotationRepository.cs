using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly ApplicationDbContext _context;

        public QuotationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Quotation>> GetAllAsync()
        {
            return await _context.Quotations
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateAsync(Quotation quotation)
        {
            _context.Quotations.Add(quotation);
        }

        public async Task<Quotation?> GetByIdAsync(int id)
        {
            return await _context.Quotations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UserHasQuotationAsync(Guid userId)
        {
            return await _context.Quotations
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.IsAvailable);
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId && x.IsAvailable);
        }
    }
}
