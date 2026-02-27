using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class LookupDetailsRepository : ILookupDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public LookupDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LookupDetails>> GetByMasterCodeAsync(string masterCode)
        {
            var normalized = (masterCode ?? string.Empty).Trim().ToUpperInvariant();
            return await _context.LookupDetails
                .AsNoTracking()
                .Where(x => x.MasterCode.ToUpper() == normalized && x.IsAvailable)
                .OrderBy(x => x.NameEn)
                .ToListAsync();
        }

        public async Task<LookupDetails?> GetByMasterAndDetailAsync(string masterCode, string detailCode)
        {
            var normalizedMaster = (masterCode ?? string.Empty).Trim().ToUpperInvariant();
            var normalizedDetail = (detailCode ?? string.Empty).Trim().ToUpperInvariant();

            return await _context.LookupDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MasterCode.ToUpper() == normalizedMaster &&
                    x.DetailCode.ToUpper() == normalizedDetail &&
                    x.IsAvailable);
        }
    }
}
