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
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Quotation>> GetAllByBranchAsync(int branchId)
        {
            return await _context.Quotations
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Where(x => x.Car.BranchId == branchId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public Task CreateAsync(Quotation quotation)
        {
            _context.Quotations.Add(quotation);
            return Task.CompletedTask;
        }

        public async Task<Quotation?> GetByIdAsync(int id)
        {
            return await _context.Quotations
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Quotation?> GetByIdAsync(int id, int branchId)
        {
            return await _context.Quotations
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .FirstOrDefaultAsync(x => x.Id == id && x.Car.BranchId == branchId);
        }

        public async Task<Quotation?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Quotations
                .Include(x => x.Car)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Quotation?> GetByIdForUpdateAsync(int id, int branchId)
        {
            return await _context.Quotations
                .Include(x => x.Car)
                .FirstOrDefaultAsync(x => x.Id == id && x.Car.BranchId == branchId);
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
