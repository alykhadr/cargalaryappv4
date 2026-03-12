using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class RequestHistoryRepository : IRequestHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task CreateAsync(RequestHistory requestHistory)
        {
            _context.RequestHistories.Add(requestHistory);
            return Task.CompletedTask;
        }

        public async Task<List<RequestHistory>> GetByRequestIdAsync(int requestId)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .Where(x => x.RequestId == requestId)
                .OrderByDescending(x => x.StatusDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsByRequestAndStatusAsync(int requestId, int status)
        {
            return await _context.RequestHistories
                .AsNoTracking()
                .AnyAsync(x => x.RequestId == requestId && x.Status == status);
        }
    }
}
