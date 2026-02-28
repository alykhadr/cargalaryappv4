using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class QuotationHistoryRepository : IQuotationHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public QuotationHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task CreateAsync(QuotationHistory quotationHistory)
        {
            _context.QuotationHistories.Add(quotationHistory);
            return Task.CompletedTask;
        }

        public async Task<List<QuotationHistory>> GetByQuotationIdAsync(int quotationId)
        {
            return await _context.QuotationHistories
                .AsNoTracking()
                .Where(x => x.QuotationId == quotationId)
                .OrderByDescending(x => x.StatusDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsByQuotationAndStatusAsync(int quotationId, int status)
        {
            return await _context.QuotationHistories
                .AsNoTracking()
                .AnyAsync(x => x.QuotationId == quotationId && x.Status == status);
        }
    }
}
