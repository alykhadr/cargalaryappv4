using CarGalary.Admin.Api.Security;
using CarGalary.Application.Interfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DashboardController(
            ICarService carService,
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _carService = carService;
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet("cars-by-created-date")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetCarsByCreatedDate([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 100);
            var cars = await _carService.GetAllAsync();

            var orderedCars = cars
                .Where(x => x.IsAvailable)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            var totalCount = orderedCars.Count;
            var pageItems = orderedCars
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToList();

            var carIds = pageItems.Select(x => x.Id).Distinct().ToList();
            var requestCounts = carIds.Count == 0
                ? new Dictionary<int, int>()
                : await _context.Requests
                    .Where(r => r.IsAvailable && carIds.Contains(r.CarId))
                    .GroupBy(r => r.CarId)
                    .Select(g => new { CarId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.CarId, x => x.Count);

            var stockTotals = carIds.Count == 0
                ? new Dictionary<int, int>()
                : await _context.CarColors
                    .Where(c => carIds.Contains(c.CarId))
                    .GroupBy(c => c.CarId)
                    .Select(g => new { CarId = g.Key, TotalStock = g.Sum(x => x.StockQuantity ?? 0) })
                    .ToDictionaryAsync(x => x.CarId, x => x.TotalStock);

            var carPrimaryImages = carIds.Count == 0
                ? new Dictionary<int, string?>()
                : (await _context.CarGalleryImages
                    .Where(i =>
                        i.IsAvailable &&
                        carIds.Contains(i.CarId) &&
                        i.ImageUrl != null &&
                        i.ImageUrl != string.Empty)
                    .OrderByDescending(i => i.IsPrimary)
                    .ThenByDescending(i => i.CreatedAt)
                    .Select(i => new { i.CarId, i.ImageUrl })
                    .ToListAsync())
                    .GroupBy(i => i.CarId)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ImageUrl).FirstOrDefault());

            var items = pageItems.Select(car => new
            {
                car.Id,
                car.NameAr,
                car.NameEn,
                car.CreatedAt,
                car.IsAvailable,
                car.Year,
                PrimaryImageUrl = carPrimaryImages.TryGetValue(car.Id, out var primaryImageUrl) ? primaryImageUrl : null,
                RequestsCount = requestCounts.TryGetValue(car.Id, out var requestsCount) ? requestsCount : 0,
                TotalStock = stockTotals.TryGetValue(car.Id, out var totalStock) ? totalStock : 0
            });

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                items
            });
        }

        [HttpGet("brands")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetBrands([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 100);

            var orderedBrands = await _context.Brands
                .Where(x => x.IsAvailable)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.NameAr,
                    x.NameEn,
                    x.ImageUrl,
                    x.CreatedAt,
                    x.IsAvailable
                })
                .ToListAsync();

            var totalCount = orderedBrands.Count;
            var pageItems = orderedBrands
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToList();

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                items = pageItems
            });
        }

        [HttpGet("recent-requests")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetRecentRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 100);
            var userBranchId = _currentUserService.BranchId;

            var query = _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(x => x.CurrentStatusLookup)
                .Where(x => x.IsAvailable)
                .AsQueryable();

            if (userBranchId.HasValue)
            {
                query = query.Where(x => x.Car.BranchId == userBranchId.Value);
            }

            var totalCount = await query.CountAsync();

            var pageItems = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Email,
                    x.MobileNo,
                    x.CreatedAt,
                    x.CurrentStatus,
                    x.CurrentStatusDate,
                    x.CarId,
                    CarNameAr = x.Car.NameAr,
                    CarNameEn = x.Car.NameEn,
                    CurrentStatusNameAr = x.CurrentStatusLookup != null ? x.CurrentStatusLookup.NameAr : null,
                    CurrentStatusNameEn = x.CurrentStatusLookup != null ? x.CurrentStatusLookup.NameEn : null,
                    CurrentStatusCode = x.CurrentStatusLookup != null ? x.CurrentStatusLookup.DetailCode : null
                })
                .ToListAsync();

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                items = pageItems
            });
        }
    }
}
