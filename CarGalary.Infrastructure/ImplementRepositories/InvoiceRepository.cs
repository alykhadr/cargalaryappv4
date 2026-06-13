using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await BaseQuery()
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetAllByBranchAsync(int branchId)
        {
            return await BaseQuery()
                .AsNoTracking()
                .Where(x => x.BranchId == branchId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await BaseQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Invoice?> GetByIdAsync(int id, int branchId)
        {
            return await BaseQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.BranchId == branchId);
        }

        public async Task<Invoice?> GetByIdForUpdateAsync(int id)
        {
            return await BaseQuery().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Invoice?> GetByIdForUpdateAsync(int id, int branchId)
        {
            return await BaseQuery().FirstOrDefaultAsync(x => x.Id == id && x.BranchId == branchId);
        }

        public Task CreateAsync(Invoice invoice)
        {
            _context.Set<Invoice>().Add(invoice);
            return Task.CompletedTask;
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId && x.IsAvailable);
        }

        public async Task<bool> InvoiceNumberExistsAsync(string invoiceNumber, int? excludeInvoiceId = null)
        {
            var query = _context.Set<Invoice>()
                .AsNoTracking()
                .Where(x => x.InvoiceNumber == invoiceNumber);

            if (excludeInvoiceId.HasValue)
            {
                query = query.Where(x => x.Id != excludeInvoiceId.Value);
            }

            return await query.AnyAsync();
        }

        private IQueryable<Invoice> BaseQuery()
        {
            return _context.Set<Invoice>()
                .Include(x => x.User)
                .Include(x => x.Branch)
                .Include(x => x.PaymentMethodLookup)
                .Include(x => x.Details.Where(d => d.IsAvailable))
                    .ThenInclude(x => x.Car)
                        .ThenInclude(x => x.CarModel)
                            .ThenInclude(x => x.Brand)
                .Include(x => x.Details.Where(d => d.IsAvailable))
                    .ThenInclude(x => x.Car)
                        .ThenInclude(x => x.CarImages)
                .Include(x => x.Details.Where(d => d.IsAvailable))
                    .ThenInclude(x => x.Color);
        }
    }
}
