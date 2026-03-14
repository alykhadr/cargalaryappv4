using CarGalary.Admin.Api.Security;
using CarGalary.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("customer-reviews")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetCustomerReviews()
        {
            var query = _context.UserRates
                .AsNoTracking()
                .Where(x => x.IsAvailable && !x.IsProductReview);

            var totalReviews = await query.CountAsync();
            var averageRating = totalReviews == 0
                ? 0m
                : Math.Round(await query.AverageAsync(x => x.RateValue), 2);

            var grouped = await query
                .GroupBy(x => (int)Math.Floor(x.RateValue))
                .Select(g => new
                {
                    Star = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var distribution = Enumerable.Range(1, 5)
                .Select(star =>
                {
                    var count = grouped
                        .Where(x => x.Star == star)
                        .Select(x => x.Count)
                        .FirstOrDefault();

                    var percentage = totalReviews == 0
                        ? 0m
                        : Math.Round((decimal)count * 100m / totalReviews, 2);

                    return new
                    {
                        star,
                        count,
                        percentage
                    };
                })
                .OrderByDescending(x => x.star)
                .ToList();

            var latestItems = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.RateValue,
                    x.CreatedAt,
                    x.CommentAr,
                    x.CommentEn,
                    ReviewerNameAr = x.ReviewerNameAr ?? (x.User != null ? x.User.FullNameAr : null),
                    ReviewerNameEn = x.ReviewerNameEn ?? (x.User != null ? x.User.FullNameEn : null)
                })
                .ToListAsync();

            return Ok(new
            {
                totalReviews,
                averageRating,
                distribution,
                items = latestItems
            });
        }

        [HttpGet("product-reviews")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetProductReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);

            var query = _context.UserRates
                .AsNoTracking()
                .Where(x => x.IsAvailable && x.IsProductReview);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.CarId,
                    x.RateValue,
                    x.CreatedAt,
                    x.CommentAr,
                    x.CommentEn,
                    ReviewerNameAr = x.ReviewerNameAr ?? (x.User != null ? x.User.FullNameAr : null),
                    ReviewerNameEn = x.ReviewerNameEn ?? (x.User != null ? x.User.FullNameEn : null),
                    CarNameAr = x.Car != null ? x.Car.NameAr : null,
                    CarNameEn = x.Car != null ? x.Car.NameEn : null
                })
                .ToListAsync();

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
