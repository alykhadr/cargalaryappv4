using CarGalary.Domain.Entities;
using CarGalary.Domain.RepositoryInterfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Infrastructure.ImplementRepositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Request>> GetAllAsync()
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Request>> GetAllByBranchAsync(int branchId)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .Where(x => x.Car.BranchId == branchId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public Task CreateAsync(Request request)
        {
            _context.Requests.Add(request);
            return Task.CompletedTask;
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Request?> GetByIdAsync(int id, int branchId)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .FirstOrDefaultAsync(x => x.Id == id && x.Car.BranchId == branchId);
        }

        public async Task<Request?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Requests
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Request?> GetByIdForUpdateAsync(int id, int branchId)
        {
            return await _context.Requests
                .Include(x => x.Car)
                .ThenInclude(x => x.CarColors)
                .ThenInclude(x => x.ColorStatusLookup)
                .Include(x => x.Color)
                .Include(x => x.CurrentStatusLookup)
                .FirstOrDefaultAsync(x => x.Id == id && x.Car.BranchId == branchId);
        }

        public async Task<bool> UserHasRequestAsync(Guid userId)
        {
            return await _context.Requests
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
