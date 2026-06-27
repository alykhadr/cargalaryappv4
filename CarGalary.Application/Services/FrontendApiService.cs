using System.Globalization;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Application.Dtos.Frontend;
using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Request.Query;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace CarGalary.Application.Services
{
    public class FrontendApiService : IFrontendApiService
    {
        private const string DefaultPaymentMethodDetailCode = "1";
        private const string DefaultVehicleOwnerTypeDetailCode = "1";
        private const string DefaultRegionDetailCode = "1";
        private const string DefaultCityDetailCode = "1001";

        private readonly ICarService _carService;
        private readonly IFavoritesService _favoritesService;
        private readonly IRequestService _requestService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FrontendApiService(
            ICarService carService,
            IFavoritesService favoritesService,
            IRequestService requestService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _carService = carService;
            _favoritesService = favoritesService;
            _requestService = requestService;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<FrontendCarsResponseDto> GetCarsAsync(FrontendCarQueryDto query)
        {
            query ??= new FrontendCarQueryDto();

            var cars = (await _carService.GetAllForApiAsync())
                .Where(x => x.IsAvailable)
                .Select(ToFrontendCar)
                .ToList();

            IEnumerable<FrontendCarDto> filtered = cars;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();
                filtered = filtered.Where(x =>
                    ContainsIgnoreCase(x.Name, search) ||
                    ContainsIgnoreCase(x.Brand, search) ||
                    ContainsIgnoreCase(x.Description, search));
            }

            if (int.TryParse(query.CategoryId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var categoryId) &&
                categoryId > 0)
            {
                filtered = filtered.Where(x =>
                    int.TryParse(x.CategoryId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var carCategoryId) &&
                    carCategoryId == categoryId);
            }

            if (TryParseDecimal(query.MinPrice, out var minPrice))
            {
                filtered = filtered.Where(x => TryParseDecimal(x.Price, out var price) && price >= minPrice);
            }

            if (TryParseDecimal(query.MaxPrice, out var maxPrice))
            {
                filtered = filtered.Where(x => TryParseDecimal(x.Price, out var price) && price <= maxPrice);
            }

            filtered = ApplySort(filtered, query.SortBy);

            var total = filtered.Count();
            var offset = ParsePositiveInt(query.Offset);
            var limit = ParsePositiveInt(query.Limit);

            if (offset > 0)
            {
                filtered = filtered.Skip(offset);
            }

            if (limit > 0)
            {
                filtered = filtered.Take(Math.Min(limit, 100));
            }

            return new FrontendCarsResponseDto
            {
                Cars = filtered.ToList(),
                Total = total
            };
        }

        public async Task<FrontendCarDto?> GetCarAsync(int id)
        {
            var car = await _carService.GetByIdForApiAsync(id);
            return car == null || !car.IsAvailable ? null : ToFrontendCar(car);
        }

        public async Task<List<FrontendCarDto>> GetWishlistAsync(Guid userId)
        {
            var favorites = await _favoritesService.GetAllAsync();
            var favoriteIds = favorites
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.CarId)
                .Distinct()
                .ToList();

            if (favoriteIds.Count == 0)
            {
                return new List<FrontendCarDto>();
            }

            var cars = (await GetCarsAsync(new FrontendCarQueryDto())).Cars
                .Where(x => int.TryParse(x.Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                .ToDictionary(x => int.Parse(x.Id, CultureInfo.InvariantCulture));

            return favoriteIds
                .Where(cars.ContainsKey)
                .Select(id => cars[id])
                .ToList();
        }

        public async Task<bool> IsWishlistedAsync(Guid userId, int carId)
        {
            return await _favoritesService.GetByIdAsync(userId, carId) != null;
        }

        public async Task AddToWishlistAsync(Guid userId, int carId)
        {
            var car = await _carService.GetByIdForApiAsync(carId);
            if (car == null || !car.IsAvailable)
            {
                throw new ArgumentException("CarId is invalid");
            }

            if (await IsWishlistedAsync(userId, carId))
            {
                return;
            }

            await _favoritesService.CreateAsync(new CreateUserFavoriteAdminRequestDto
            {
                UserId = userId,
                CarId = carId,
                Priority = 0
            });
        }

        public async Task RemoveFromWishlistAsync(Guid userId, int carId)
        {
            if (!await IsWishlistedAsync(userId, carId))
            {
                return;
            }

            await _favoritesService.DeleteAsync(userId, carId);
        }

        public async Task<List<FrontendInquiryWithCarDto>> GetInquiriesAsync(Guid userId)
        {
            var requests = (await _requestService.GetAllAsync())
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            if (requests.Count == 0)
            {
                return new List<FrontendInquiryWithCarDto>();
            }

            var cars = (await GetCarsAsync(new FrontendCarQueryDto())).Cars
                .Where(x => int.TryParse(x.Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                .ToDictionary(x => int.Parse(x.Id, CultureInfo.InvariantCulture));

            return requests
                .Select(x =>
                {
                    var inquiry = ToFrontendInquiry(x);
                    return new FrontendInquiryWithCarDto
                    {
                        Id = inquiry.Id,
                        UserId = inquiry.UserId,
                        CarId = inquiry.CarId,
                        Name = inquiry.Name,
                        Phone = inquiry.Phone,
                        Message = inquiry.Message,
                        CreatedAt = inquiry.CreatedAt,
                        Car = cars.TryGetValue(x.CarId, out var car) ? car : null
                    };
                })
                .ToList();
        }

        public async Task<FrontendInquiryDto> CreateInquiryAsync(
            Guid userId,
            string email,
            FrontendCreateInquiryRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException("Inquiry payload is required");
            }

            if (!int.TryParse(request.CarId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var carId) ||
                carId <= 0)
            {
                throw new ArgumentException("CarId is invalid");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                throw new ArgumentException("Phone is required");
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("Message is required");
            }

            var car = await _carService.GetByIdForApiAsync(carId);
            if (car == null || !car.IsAvailable)
            {
                throw new ArgumentException("CarId is invalid");
            }

            var colorId = car.Colors
                .Where(x => x.IsAvailable)
                .OrderByDescending(x => (x.StockQuantity ?? 0) > 0)
                .ThenBy(x => x.ColorId)
                .Select(x => x.ColorId)
                .FirstOrDefault();

            if (colorId <= 0)
            {
                throw new ArgumentException("Selected car has no available colors");
            }

            var createRequest = new CreateRequestDto
            {
                UserId = userId,
                Name = request.Name.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? $"{userId:N}@cargalary.local" : email.Trim(),
                MobileNo = request.Phone.Trim(),
                CarId = carId,
                ColorId = colorId,
                PaymentMethod = await ResolveLookupIdAsync("PAYMENT_METHOD", DefaultPaymentMethodDetailCode),
                RegionId = await ResolveLookupIdAsync("REGION", DefaultRegionDetailCode),
                CityId = await ResolveLookupIdAsync("CITY", DefaultCityDetailCode),
                VehicleOwnerType = await ResolveLookupIdAsync("VEHICLE_OWNER_TYPE", DefaultVehicleOwnerTypeDetailCode),
                Notes = request.Message.Trim()
            };

            var created = await _requestService.CreateAsync(createRequest);
            return ToFrontendInquiry(created);
        }

        private FrontendCarDto ToFrontendCar(CarApiResponseDto car)
        {
            var brandName = FirstNonEmpty(car.BrandNameEn, car.BrandNameAr, "Unknown");
            var modelName = FirstNonEmpty(car.ModelNameEn, car.ModelNameAr);
            var name = FirstNonEmpty(car.NameEn, car.NameAr, string.Join(" ", new[] { brandName, modelName }.Where(x => !string.IsNullOrWhiteSpace(x))));
            var description = FirstNonEmpty(car.DescriptionEn, car.DescriptionAr);
            var brandLogoKey = NormalizeBrandLogoKey(brandName);

            var galleryImages = car.GalleryImages
                .Where(x => x.IsAvailable && !string.IsNullOrWhiteSpace(x.ImageUrl))
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.Id)
                .Select(x => NormalizeAssetUrl(x.ImageUrl))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var colorImages = car.Colors
                .Where(x => x.IsAvailable && !string.IsNullOrWhiteSpace(x.ColorImageUrl))
                .OrderByDescending(x => (x.StockQuantity ?? 0) > 0)
                .ThenBy(x => x.ColorId)
                .Select(x => NormalizeAssetUrl(x.ColorImageUrl))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var curatedImages = BuildShowcaseImageUrls(brandLogoKey, car.Id);
            var sliderImages = curatedImages.Count > 0
                ? curatedImages
                : galleryImages.Count > 0
                    ? galleryImages
                    : colorImages;
            var imageKey = sliderImages.FirstOrDefault() ?? string.Empty;

            var colors = car.Colors
                .Where(x => x.IsAvailable)
                .Select(x => FirstNonEmpty(x.ColorCode, x.ColorNameEn, x.ColorNameAr))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var prices = car.Colors
                .Where(x => x.IsAvailable)
                .Select(x => x.TotalPrice ?? x.PricingPerColor ?? x.PricePefore)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList();

            var price = prices.Count > 0 ? prices.Min() : 0m;
            var showcaseStats = BuildShowcaseStats(car.Id, brandName);

            return new FrontendCarDto
            {
                Id = car.Id.ToString(CultureInfo.InvariantCulture),
                Name = name,
                Model = modelName,
                ImageKey = imageKey,
                SliderImageKeys = sliderImages,
                Price = price.ToString("N2", CultureInfo.InvariantCulture),
                NumReviews = showcaseStats.NumReviews,
                Rating = showcaseStats.Rating,
                NumSolds = showcaseStats.NumSolds,
                CategoryId = car.BrandId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                Brand = brandName,
                BrandLogoKey = brandLogoKey,
                Description = description,
                Colors = colors,
                Year = car.Year,
                Mileage = car.Mileage,
                FuelType = FirstNonEmpty(car.FuelTypeNameEn, car.FuelTypeNameAr),
                Transmission = FirstNonEmpty(car.TransmisionTypeNameEn, car.TransmisionTypeNameAr),
                Drivetrain = FirstNonEmpty(car.DrivetrainNameEn, car.DrivetrainNameAr),
                VehicleClass = FirstNonEmpty(car.VehicleClassNameEn, car.VehicleClassNameAr, car.TypeNameEn, car.TypeNameAr),
                SeatingCapacity = car.SeatingCapacity ?? 0,
                CreatedAtUtc = car.CreatedAt,
            };
        }

        private static FrontendInquiryDto ToFrontendInquiry(RequestResponseDto request)
        {
            return new FrontendInquiryDto
            {
                Id = request.Id.ToString(CultureInfo.InvariantCulture),
                UserId = request.UserId?.ToString() ?? string.Empty,
                CarId = request.CarId.ToString(CultureInfo.InvariantCulture),
                Name = request.Name,
                Phone = request.MobileNo,
                Message = request.Notes ?? string.Empty,
                CreatedAt = request.CreatedAt.ToString("O", CultureInfo.InvariantCulture)
            };
        }

        private string NormalizeAssetUrl(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var trimmed = value.Trim();
            if (Uri.TryCreate(trimmed, UriKind.Absolute, out _))
            {
                return trimmed;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return trimmed;
            }

            var path = trimmed.StartsWith('/') ? trimmed : $"/{trimmed}";
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{path}";
        }

        private async Task<int> ResolveLookupIdAsync(string masterCode, params string[] preferredDetailCodes)
        {
            var values = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);

            foreach (var preferred in preferredDetailCodes.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var match = values.FirstOrDefault(x =>
                    string.Equals(x.DetailCode, preferred, StringComparison.OrdinalIgnoreCase) ||
                    x.Id.ToString(CultureInfo.InvariantCulture) == preferred);

                if (match != null)
                {
                    return match.Id;
                }
            }

            var fallback = values.OrderBy(x => x.Id).FirstOrDefault();
            if (fallback == null)
            {
                throw new ArgumentException($"{masterCode} is not configured");
            }

            return fallback.Id;
        }

        private static IEnumerable<FrontendCarDto> ApplySort(IEnumerable<FrontendCarDto> cars, string? sortBy)
        {
            return sortBy?.Trim().ToLowerInvariant() switch
            {
                "recent" => cars.OrderByDescending(x => x.CreatedAtUtc),
                "price_asc" => cars.OrderBy(x => TryParseDecimal(x.Price, out var price) ? price : decimal.MaxValue),
                "price_desc" => cars.OrderByDescending(x => TryParseDecimal(x.Price, out var price) ? price : decimal.MinValue),
                "name" => cars.OrderBy(x => x.Name),
                "popular" => cars.OrderByDescending(x => x.NumSolds),
                "rating" => cars.OrderByDescending(x => x.Rating),
                _ => cars
            };
        }

        private static bool ContainsIgnoreCase(string? value, string search)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private static string FirstNonEmpty(params string?[] values)
        {
            return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;
        }

        private static string NormalizeBrandLogoKey(string brandName)
        {
            return brandName.Trim().ToLowerInvariant().Replace(" ", "-");
        }

        private static ShowcaseStats BuildShowcaseStats(int carId, string brandName)
        {
            var hash = Math.Abs(HashCode.Combine(carId, brandName));
            var ratings = new[] { 4.3m, 4.4m, 4.5m, 4.6m, 4.7m, 4.8m, 4.9m };

            return new ShowcaseStats(
                ratings[hash % ratings.Length],
                18 + (hash % 64),
                6 + (hash % 42));
        }

        private static List<string> BuildShowcaseImageUrls(string brandLogoKey, int carId)
        {
            var supportedBrands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "bmw",
                "bugatti",
                "honda",
                "mercedes",
                "tesla",
                "toyota",
                "volvo",
            };

            if (!supportedBrands.Contains(brandLogoKey))
            {
                return new List<string>();
            }

            var keys = new List<string>();
            var start = Math.Abs(carId % 12) + 1;

            for (var offset = 0; offset < 3; offset += 1)
            {
                var imageIndex = ((start + offset - 1) % 12) + 1;
                keys.Add($"/uploads/showcase/{brandLogoKey}/{brandLogoKey}{imageIndex}.png");
            }

            return keys;
        }

        private static int ParsePositiveInt(string? value)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) && parsed > 0
                ? parsed
                : 0;
        }

        private static bool TryParseDecimal(string? value, out decimal parsed)
        {
            return decimal.TryParse(
                value?.Replace(",", string.Empty),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out parsed);
        }

        private readonly record struct ShowcaseStats(decimal Rating, int NumReviews, int NumSolds);
    }
}
