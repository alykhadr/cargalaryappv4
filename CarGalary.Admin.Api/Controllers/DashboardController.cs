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

        public DashboardController(
            ICarService carService,
            ApplicationDbContext context)
        {
            _carService = carService;
            _context = context;
        }

        [HttpGet("cars-by-created-date")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetCarsByCreatedDate([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 100);
            var cars = await _carService.GetAllAsync();

            var orderedCars = cars
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

            var items = pageItems.Select(car => new
            {
                car.Id,
                car.NameAr,
                car.NameEn,
                car.CreatedAt,
                car.IsAvailable,
                car.Year,
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
    }
}
