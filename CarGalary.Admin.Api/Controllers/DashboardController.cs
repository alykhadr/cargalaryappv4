using CarGalary.Admin.Api.Security;
using CarGalary.Application.Interfaces;
using CarGalary.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

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

        [HttpGet("favorite-cars")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetFavoriteCars([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 100);
            var userBranchId = _currentUserService.BranchId;

            var query = _context.UserFavorites
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(x => x.User)
                .Where(x => x.Car.IsAvailable)
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
                    x.CarId,
                    x.UserId,
                    x.CreatedAt,
                    x.Priority,
                    x.Notes,
                    CarNameAr = x.Car.NameAr,
                    CarNameEn = x.Car.NameEn,
                    UserName = x.User.UserName,
                    FullNameAr = x.User.FullNameAr,
                    FullNameEn = x.User.FullNameEn
                })
                .ToListAsync();

            var carIds = pageItems.Select(x => x.CarId).Distinct().ToList();
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

            var items = pageItems.Select(x => new
            {
                x.CarId,
                x.UserId,
                x.CreatedAt,
                x.Priority,
                x.Notes,
                x.CarNameAr,
                x.CarNameEn,
                x.UserName,
                x.FullNameAr,
                x.FullNameEn,
                PrimaryImageUrl = carPrimaryImages.TryGetValue(x.CarId, out var primaryImageUrl) ? primaryImageUrl : null
            });

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                items
            });
        }

        [HttpGet("offers")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetOffers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);
            var utcNow = DateTime.UtcNow;

            var query = _context.Offers
                .AsNoTracking()
                .Where(x =>
                    x.IsAvailable &&
                    (x.ExpiredAt == null || x.ExpiredAt >= utcNow))
                .OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.OfferNameAr,
                    x.OfferNameEn,
                    x.DescriptionAr,
                    x.DescriptionEn,
                    x.OfferImageUrl,
                    x.ExpiredAt,
                    x.CreatedAt
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

        [HttpGet("member-services")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetMemberServices([FromQuery] int page = 1, [FromQuery] int pageSize = 6)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 6 : Math.Min(pageSize, 100);

            var query = _context.MemberServices
                .AsNoTracking()
                .Where(x => x.IsAvailable)
                .OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.NameAr,
                    x.NameEn,
                    x.DescriptionAr,
                    x.DescriptionEn,
                    x.ImageUrl,
                    x.CreatedAt
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

        [HttpGet("sales-contacts")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetSalesContacts([FromQuery] int page = 1, [FromQuery] int pageSize = 6)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 6 : Math.Min(pageSize, 100);
            var userBranchId = _currentUserService.BranchId;

            var query = _context.ContactSalesOfficers
                .AsNoTracking()
                .Where(x => x.IsAvailable)
                .AsQueryable();

            if (userBranchId.HasValue)
            {
                query = query.Where(x => x.BranchId == userBranchId.Value);
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.ContactValue,
                    x.ContactType,
                    x.ContactIconUrl,
                    x.BranchId,
                    x.CreatedAt
                })
                .ToListAsync();

            var lookupRows = await _context.LookupDetails
                .AsNoTracking()
                .Where(x => x.IsAvailable && x.MasterCode.ToUpper() == "CONTACT_TYPE")
                .Select(x => new
                {
                    x.DetailCode,
                    x.NameAr,
                    x.NameEn
                })
                .ToListAsync();

            var lookupMap = lookupRows
                .Select(x => new
                {
                    Parsed = int.TryParse(x.DetailCode, out var parsedCode) ? parsedCode : (int?)null,
                    x.NameAr,
                    x.NameEn
                })
                .Where(x => x.Parsed.HasValue)
                .ToDictionary(
                    x => x.Parsed!.Value,
                    x => new
                    {
                        x.NameAr,
                        x.NameEn
                    });

            var items = pageItems.Select(x =>
            {
                var typeLabel = lookupMap.TryGetValue(x.ContactType, out var typeInfo)
                    ? typeInfo
                    : null;

                return new
                {
                    x.Id,
                    x.ContactValue,
                    x.ContactType,
                    x.ContactIconUrl,
                    x.BranchId,
                    x.CreatedAt,
                    TypeNameAr = typeLabel?.NameAr ?? x.ContactType.ToString(CultureInfo.InvariantCulture),
                    TypeNameEn = typeLabel?.NameEn ?? x.ContactType.ToString(CultureInfo.InvariantCulture)
                };
            });

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                items
            });
        }

        [HttpGet("active-employees")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetActiveEmployees(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string scope = "branch",
            [FromQuery] int? branchId = null)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 6 : Math.Min(pageSize, 100);
            var requestedScope = (scope ?? "branch").Trim().ToLowerInvariant();
            var canViewAllBranches = _currentUserService.IsInRole("Admin") || _currentUserService.IsInRole("Manager");
            var normalizedScope = canViewAllBranches && requestedScope == "all" ? "all" : "branch";
            var userBranchId = _currentUserService.BranchId;

            var query = _context.Employees
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Department)
                .Where(x =>
                    x.IsAvailable &&
                    x.User != null &&
                    x.User.IsAvailable)
                .AsQueryable();

            if (normalizedScope == "branch")
            {
                var resolvedBranchId = canViewAllBranches && branchId.HasValue && branchId.Value > 0
                    ? branchId
                    : userBranchId;

                if (!resolvedBranchId.HasValue || resolvedBranchId.Value <= 0)
                {
                    return Ok(new
                    {
                        page = safePage,
                        pageSize = safePageSize,
                        totalCount = 0,
                        scope = "branch",
                        branchId = (int?)null,
                        canViewAllBranches,
                        items = Array.Empty<object>()
                    });
                }

                query = query.Where(x => x.BranchId == resolvedBranchId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.BranchId,
                    x.CreatedAt,
                    ProfileImageUrl = x.User != null ? x.User.ProfileImageUrl : null,
                    FullNameAr = x.User != null ? x.User.FullNameAr : null,
                    FullNameEn = x.User != null ? x.User.FullNameEn : null,
                    DepartmentNameAr = x.Department != null ? x.Department.NameAr : null,
                    DepartmentNameEn = x.Department != null ? x.Department.NameEn : null
                })
                .ToListAsync();

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount,
                scope = normalizedScope,
                branchId = normalizedScope == "branch"
                    ? (canViewAllBranches && branchId.HasValue && branchId.Value > 0 ? branchId.Value : userBranchId)
                    : (int?)null,
                canViewAllBranches,
                items
            });
        }

        [HttpGet("best-seller-cars")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetBestSellerCars([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var safePage = 1;
            var safePageSize = 5;
            var userBranchId = _currentUserService.BranchId;

            var query = _context.Requests
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(x => x.CurrentStatusLookup)
                .Where(x =>
                    x.IsAvailable &&
                    x.Car.IsAvailable &&
                    x.CurrentStatusLookup != null &&
                    x.CurrentStatusLookup.DetailCode == "4")
                .AsQueryable();

            if (userBranchId.HasValue)
            {
                query = query.Where(x => x.Car.BranchId == userBranchId.Value);
            }

            var groupedQuery = query
                .GroupBy(x => x.CarId)
                .Select(g => new
                {
                    CarId = g.Key,
                    SalesCount = g.Count(),
                    LastSoldAt = g.Max(x => x.CurrentStatusDate ?? x.CreatedAt)
                })
                .Join(
                    _context.Cars
                        .AsNoTracking()
                        .Select(car => new
                        {
                            car.Id,
                            NameAr = car.NameAr,
                            NameEn = car.NameEn
                        }),
                    sales => sales.CarId,
                    car => car.Id,
                    (sales, car) => new
                    {
                        sales.CarId,
                        car.NameAr,
                        car.NameEn,
                        sales.SalesCount,
                        sales.LastSoldAt
                    });

            var pageItems = await groupedQuery
                .OrderByDescending(x => x.SalesCount)
                .ThenByDescending(x => x.LastSoldAt)
                .Take(safePageSize)
                .ToListAsync();

            var carIds = pageItems.Select(x => x.CarId).Distinct().ToList();
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

            var items = pageItems.Select(x => new
            {
                carId = x.CarId,
                nameAr = string.IsNullOrWhiteSpace(x.NameAr) ? null : x.NameAr.Trim(),
                nameEn = string.IsNullOrWhiteSpace(x.NameEn) ? null : x.NameEn.Trim(),
                carNameAr = string.IsNullOrWhiteSpace(x.NameAr) ? null : x.NameAr.Trim(),
                carNameEn = string.IsNullOrWhiteSpace(x.NameEn) ? null : x.NameEn.Trim(),
                salesCount = x.SalesCount,
                lastSoldAt = x.LastSoldAt,
                primaryImageUrl = carPrimaryImages.TryGetValue(x.CarId, out var primaryImageUrl) ? primaryImageUrl : null
            });

            return Ok(new
            {
                page = safePage,
                pageSize = safePageSize,
                totalCount = pageItems.Count,
                items
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

        [HttpGet("request-status-counts")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetRequestStatusCounts([FromQuery] string? period = "1m")
        {
            var periodOptions = await GetAvailablePeriodOptionsAsync();
            var normalizedPeriod = NormalizePeriod(period, periodOptions);
            var userBranchId = _currentUserService.BranchId;
            var utcNow = DateTime.UtcNow;
            var fromDate = ResolveFromDate(normalizedPeriod, utcNow);

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

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= fromDate.Value);
            }

            var groupedByDetailCode = await query
                .GroupBy(x => x.CurrentStatusLookup != null ? x.CurrentStatusLookup.DetailCode : null)
                .Select(g => new
                {
                    DetailCode = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            int CountByCode(string code) => groupedByDetailCode
                .Where(x => string.Equals(x.DetailCode, code, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Count)
                .FirstOrDefault();

            var total = groupedByDetailCode.Sum(x => x.Count);
            var closedSuccessCount = CountByCode("4");
            var conversionRatio = total == 0
                ? 0m
                : Math.Round((decimal)closedSuccessCount * 100m / total, 2);

            return Ok(new
            {
                period = normalizedPeriod,
                periodOptions = periodOptions.Select(x => new
                {
                    code = x.Code,
                    nameAr = x.NameAr,
                    nameEn = x.NameEn
                }),
                fromDate,
                toDate = utcNow,
                total,
                newCount = CountByCode("1"),
                contactCount = CountByCode("2"),
                inProgressCount = CountByCode("3"),
                closedSuccessCount,
                closedLossCount = CountByCode("5"),
                conversionRatio
            });
        }

        [HttpGet("sales-by-branches")]
        [PermissionAuthorize("dashboard.view")]
        public async Task<IActionResult> GetSalesByBranches()
        {
            var userBranchId = _currentUserService.BranchId;

            var branchQuery = _context.Branches
                .AsNoTracking()
                .Where(x => x.IsAvailable)
                .AsQueryable();

            if (userBranchId.HasValue)
            {
                branchQuery = branchQuery.Where(x => x.Id == userBranchId.Value);
            }

            var branches = await branchQuery
                .Select(x => new
                {
                    x.Id,
                    x.BranchNameAr,
                    x.BranchNameEn,
                    x.Latitute,
                    x.Longtute
                })
                .ToListAsync();

            if (branches.Count == 0)
            {
                return Ok(new { totalSales = 0, items = Array.Empty<object>() });
            }

            var branchIds = branches.Select(x => x.Id).Distinct().ToList();
            var salesByBranch = await _context.Requests
                .AsNoTracking()
                .Where(x =>
                    x.IsAvailable &&
                    branchIds.Contains(x.Car.BranchId) &&
                    x.CurrentStatusLookup != null &&
                    x.CurrentStatusLookup.DetailCode == "4")
                .GroupBy(x => x.Car.BranchId)
                .Select(g => new
                {
                    BranchId = g.Key,
                    SalesCount = g.Count()
                })
                .ToListAsync();

            var salesMap = salesByBranch.ToDictionary(x => x.BranchId, x => x.SalesCount);
            var branchSalesRaw = branches
                .Select(branch =>
                {
                    var salesCount = salesMap.TryGetValue(branch.Id, out var value) ? value : 0;

                    var hasLatitude = decimal.TryParse(
                        (branch.Latitute ?? string.Empty).Trim(),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var latitude);
                    var hasLongitude = decimal.TryParse(
                        (branch.Longtute ?? string.Empty).Trim(),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var longitude);

                    return new
                    {
                        branchId = branch.Id,
                        nameAr = branch.BranchNameAr,
                        nameEn = branch.BranchNameEn,
                        salesCount,
                        latitude = hasLatitude ? (double?)latitude : null,
                        longitude = hasLongitude ? (double?)longitude : null
                    };
                })
                .OrderByDescending(x => x.salesCount)
                .ThenBy(x => x.branchId)
                .Take(3)
                .ToList();

            var totalSales = branchSalesRaw.Sum(x => x.salesCount);
            var items = branchSalesRaw.Select(x => new
            {
                x.branchId,
                x.nameAr,
                x.nameEn,
                x.salesCount,
                percentage = totalSales == 0
                    ? 0m
                    : Math.Round((decimal)x.salesCount * 100m / totalSales, 2),
                x.latitude,
                x.longitude
            }).ToList();

            return Ok(new
            {
                totalSales,
                items
            });
        }

        private async Task<List<PeriodOption>> GetAvailablePeriodOptionsAsync()
        {
            var rows = await _context.LookupDetails
                .AsNoTracking()
                .Where(x => x.IsAvailable && x.MasterCode.ToUpper() == "DASHBOARD_PERIOD")
                .Select(x => new { x.DetailCode, x.NameAr, x.NameEn })
                .ToListAsync();

            var parsedRows = rows
                .Select(x =>
                {
                    var normalizedCode = (x.DetailCode ?? string.Empty).Trim().ToLowerInvariant();
                    var sortOrder = ResolveSortOrder(normalizedCode);
                    if (sortOrder <= 0)
                    {
                        return null;
                    }

                    return new PeriodOption(
                        normalizedCode,
                        string.IsNullOrWhiteSpace(x.NameAr) ? normalizedCode.ToUpperInvariant() : x.NameAr,
                        string.IsNullOrWhiteSpace(x.NameEn) ? normalizedCode.ToUpperInvariant() : x.NameEn,
                        sortOrder
                    );
                })
                .Where(x => x != null)
                .Select(x => x!)
                .OrderBy(x => x.SortOrder)
                .ToList();

            if (parsedRows.Count > 0)
            {
                return parsedRows;
            }

            // Fallback defaults when lookup rows are not seeded yet.
            return new List<PeriodOption>
            {
                new("1w", "1W", "1W", ResolveSortOrder("1w")),
                new("2w", "2W", "2W", ResolveSortOrder("2w")),
                new("1m", "1M", "1M", ResolveSortOrder("1m")),
                new("2m", "2M", "2M", ResolveSortOrder("2m")),
                new("3m", "3M", "3M", ResolveSortOrder("3m")),
                new("6m", "6M", "6M", ResolveSortOrder("6m")),
                new("1y", "1Y", "1Y", ResolveSortOrder("1y"))
            };
        }

        private static string NormalizePeriod(string? period, IReadOnlyCollection<PeriodOption> options)
        {
            var key = (period ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(key) && options.Any(x => string.Equals(x.Code, key, StringComparison.OrdinalIgnoreCase)))
            {
                return key;
            }

            return options
                .Select(x => x.Code)
                .FirstOrDefault(x => string.Equals(x, "1m", StringComparison.OrdinalIgnoreCase))
                ?? options.First().Code;
        }

        private static DateTime? ResolveFromDate(string period, DateTime utcNow)
        {
            if (!TryParsePeriod(period, out var value, out var unit))
            {
                return utcNow.AddMonths(-1);
            }

            return unit switch
            {
                'd' => utcNow.AddDays(-value),
                'w' => utcNow.AddDays(-(value * 7)),
                'm' => utcNow.AddMonths(-value),
                'y' => utcNow.AddYears(-value),
                _ => utcNow.AddMonths(-1)
            };
        }

        private static int ResolveSortOrder(string periodCode)
        {
            if (!TryParsePeriod(periodCode, out var value, out var unit))
            {
                return int.MaxValue;
            }

            var unitWeight = unit switch
            {
                'd' => 1,
                'w' => 7,
                'm' => 30,
                'y' => 365,
                _ => 10000
            };

            return checked(unitWeight * value);
        }

        private static bool TryParsePeriod(string periodCode, out int value, out char unit)
        {
            value = 0;
            unit = '\0';

            var normalized = (periodCode ?? string.Empty).Trim().ToLowerInvariant();
            var match = Regex.Match(normalized, @"^(\d+)\s*([dwmy])$", RegexOptions.CultureInvariant);
            if (!match.Success)
            {
                return false;
            }

            if (!int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsedValue) || parsedValue <= 0)
            {
                return false;
            }

            value = parsedValue;
            unit = match.Groups[2].Value[0];
            return true;
        }

        private sealed record PeriodOption(string Code, string NameAr, string NameEn, int SortOrder);
    }
}
