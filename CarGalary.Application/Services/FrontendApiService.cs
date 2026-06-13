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

            var sliderImages = galleryImages.Count > 0 ? galleryImages : colorImages;
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

            return new FrontendCarDto
            {
                Id = car.Id.ToString(CultureInfo.InvariantCulture),
                Name = name,
                ImageKey = imageKey,
                SliderImageKeys = sliderImages,
                Price = price.ToString("N2", CultureInfo.InvariantCulture),
                NumReviews = 0,
                Rating = 0,
                NumSolds = 0,
                CategoryId = (car.BrandId ?? car.TypeId).ToString(CultureInfo.InvariantCulture),
                Brand = brandName,
                BrandLogoKey = BuildBrandLogoKey(brandName),
                Description = description,
                Colors = colors
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

        private async Task<int> ResolveLookupIdAsync(string masterCode, string preferredDetailCode)
        {
            var lookups = await _unitOfWork.LookupDetails.GetByMasterCodeAsync(masterCode);
            var lookup = lookups.FirstOrDefault(x =>
                    string.Equals(x.DetailCode, preferredDetailCode, StringComparison.OrdinalIgnoreCase)) ??
                lookups.FirstOrDefault();

            if (lookup == null)
            {
                throw new ArgumentException($"{masterCode} lookup is not configured");
            }

            return lookup.Id;
        }

        private string NormalizeAssetUrl(string? value)
        {
            var path = value?.Trim();
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            if (!path.StartsWith("/", StringComparison.Ordinal))
            {
                return path;
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return path;
            }

            return $"{request.Scheme}://{request.Host}{request.PathBase}{path}";
        }

        private static IEnumerable<FrontendCarDto> ApplySort(IEnumerable<FrontendCarDto> cars, string? sortBy)
        {
            var normalized = sortBy?.Trim().ToLowerInvariant();
            return normalized switch
            {
                "2" or "recent" or "most_recent" or "newest" => cars.OrderByDescending(x => ParsePositiveInt(x.Id)),
                "3" or "price_high" or "price_desc" => cars.OrderByDescending(x => TryParseDecimal(x.Price, out var price) ? price : 0m),
                "4" or "price_low" or "price_asc" => cars.OrderBy(x => TryParseDecimal(x.Price, out var price) ? price : 0m),
                "5" or "rated" or "rating" or "most_rated" => cars.OrderByDescending(x => x.Rating),
                _ => cars.OrderByDescending(x => x.NumSolds).ThenByDescending(x => ParsePositiveInt(x.Id))
            };
        }

        private static bool ContainsIgnoreCase(string? value, string search)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryParseDecimal(string? raw, out decimal value)
        {
            var normalized = raw?.Trim().Replace(",", string.Empty);
            return decimal.TryParse(
                normalized,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out value);
        }

        private static int ParsePositiveInt(string? raw)
        {
            return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) && value > 0
                ? value
                : 0;
        }

        private static string FirstNonEmpty(params string?[] values)
        {
            return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;
        }

        private static string BuildBrandLogoKey(string brandName)
        {
            var normalized = brandName.Trim().ToLowerInvariant();
            if (normalized.Contains("mercedes", StringComparison.OrdinalIgnoreCase)) return "mercedes";
            if (normalized.Contains("tesla", StringComparison.OrdinalIgnoreCase)) return "tesla";
            if (normalized.Contains("bmw", StringComparison.OrdinalIgnoreCase)) return "bmw";
            if (normalized.Contains("toyota", StringComparison.OrdinalIgnoreCase)) return "toyota";
            if (normalized.Contains("volvo", StringComparison.OrdinalIgnoreCase)) return "volvo";
            if (normalized.Contains("bugatti", StringComparison.OrdinalIgnoreCase)) return "bugatti";
            if (normalized.Contains("honda", StringComparison.OrdinalIgnoreCase)) return "honda";
            return normalized.Replace(" ", string.Empty).Replace("-", string.Empty);
        }
    }
}
