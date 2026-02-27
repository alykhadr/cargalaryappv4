using AutoMapper;
using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Application.Interfaces;
using CarGalary.Domain.Entities;
using CarGalary.Domain.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace CarGalary.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _environment;

        public CarService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _environment = environment;
        }

        public async Task<List<CarResponseDto>> GetAllAsync()
        {
            var cars = await _unitOfWork.Cars.GetAllAsync();
            return _mapper.Map<List<CarResponseDto>>(cars);
        }

        public async Task<CarResponseDto?> GetByIdAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            return car == null ? null : _mapper.Map<CarResponseDto>(car);
        }

        public async Task<CarResponseDto> CreateAsync(CreateCarRequestDto dto)
        {
            if (!dto.ConditionId.HasValue)
            {
                throw new Exception("ConditionId is required");
            }

            var conditionLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_CONDITION", dto.ConditionId.Value.ToString());
            if (conditionLookup == null)
            {
                throw new Exception("ConditionId is invalid");
            }

            if (!dto.TrimLevel.HasValue)
            {
                throw new Exception("TrimLevel is required");
            }

            var trimLevelLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_TRIM_LEVEL", dto.TrimLevel.Value.ToString());
            if (trimLevelLookup == null)
            {
                throw new Exception("TrimLevel is invalid");
            }

            if (!dto.VehicleClass.HasValue)
            {
                throw new Exception("VehicleClass is required");
            }

            var vehicleClassLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_VEHICLE_CLASS", dto.VehicleClass.Value.ToString());
            if (vehicleClassLookup == null)
            {
                throw new Exception("VehicleClass is invalid");
            }

            if (!dto.TransmisionType.HasValue)
            {
                throw new Exception("TransmisionType is required");
            }

            var transmisionTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_TRANSMISION_TYPE", dto.TransmisionType.Value.ToString());
            if (transmisionTypeLookup == null)
            {
                throw new Exception("TransmisionType is invalid");
            }

            if (!dto.Drivetrain.HasValue)
            {
                throw new Exception("Drivetrain is required");
            }

            var drivetrainLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_DRIVETRAIN", dto.Drivetrain.Value.ToString());
            if (drivetrainLookup == null)
            {
                throw new Exception("Drivetrain is invalid");
            }

            if (!dto.FuelType.HasValue)
            {
                throw new Exception("FuelType is required");
            }

            var fuelTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_FUEL_TYPE", dto.FuelType.Value.ToString());
            if (fuelTypeLookup == null)
            {
                throw new Exception("FuelType is invalid");
            }

            if (!dto.ManufactureCountryId.HasValue)
            {
                throw new Exception("ManufactureCountryId is required");
            }

            var manufactureCountryLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("COUNTRY", dto.ManufactureCountryId.Value.ToString());
            if (manufactureCountryLookup == null)
            {
                throw new Exception("ManufactureCountryId is invalid");
            }

            var model = await _unitOfWork.CarModels.GetByIdAsync(dto.ModelId);
            if (model == null)
            {
                throw new Exception("CarModel not found");
            }

            var type = await _unitOfWork.CarTypes.GetCarTypeById(dto.TypeId);
            if (type == null)
            {
                throw new Exception("CarType not found");
            }

            var branch = await _unitOfWork.Branches.GetByIdAsync(dto.BranchId);
            if (branch == null)
            {
                throw new Exception("Branch not found");
            }

            var entity = _mapper.Map<Car>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = _currentUserService.UserName;

            await _unitOfWork.Cars.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarResponseDto>(entity);
        }

        public async Task<CarResponseDto> CreateWithDetailsAsync(CreateCarWithDetailsRequestDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var createdCar = await CreateAsync(BuildBaseCarCreateRequest(dto));
                var payload = BuildCreateWithDetailsPayload(dto);

                await SaveFeatureAssignmentsAsync(createdCar.Id, payload.Features, payload.UserName);
                await SaveCarColorsAsync(createdCar.Id, payload.CarColors, payload.CarColorFileByColorId, dto.Vat ?? 0m);
                await SaveExtraDetailsAsync(createdCar.Id, payload.ExtraDetails, payload.UserName);
                await SaveGalleryImagesAsync(createdCar.Id, payload.GalleryMeta, payload.GalleryFiles, payload.UserName);

                await _unitOfWork.SaveChangesAsync();
                return createdCar;
            });
        }

        public async Task UpdateAsync(int id, UpdateCarRequestDto dto)
        {
            var existing = await _unitOfWork.Cars.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Car not found");
            }

            if (!dto.ConditionId.HasValue)
            {
                throw new Exception("ConditionId is required");
            }

            var conditionLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_CONDITION", dto.ConditionId.Value.ToString());
            if (conditionLookup == null)
            {
                throw new Exception("ConditionId is invalid");
            }

            if (!dto.TrimLevel.HasValue)
            {
                throw new Exception("TrimLevel is required");
            }

            var trimLevelLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_TRIM_LEVEL", dto.TrimLevel.Value.ToString());
            if (trimLevelLookup == null)
            {
                throw new Exception("TrimLevel is invalid");
            }

            if (!dto.VehicleClass.HasValue)
            {
                throw new Exception("VehicleClass is required");
            }

            var vehicleClassLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_VEHICLE_CLASS", dto.VehicleClass.Value.ToString());
            if (vehicleClassLookup == null)
            {
                throw new Exception("VehicleClass is invalid");
            }

            if (!dto.TransmisionType.HasValue)
            {
                throw new Exception("TransmisionType is required");
            }

            var transmisionTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_TRANSMISION_TYPE", dto.TransmisionType.Value.ToString());
            if (transmisionTypeLookup == null)
            {
                throw new Exception("TransmisionType is invalid");
            }

            if (!dto.Drivetrain.HasValue)
            {
                throw new Exception("Drivetrain is required");
            }

            var drivetrainLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_DRIVETRAIN", dto.Drivetrain.Value.ToString());
            if (drivetrainLookup == null)
            {
                throw new Exception("Drivetrain is invalid");
            }

            if (!dto.FuelType.HasValue)
            {
                throw new Exception("FuelType is required");
            }

            var fuelTypeLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("CAR_FUEL_TYPE", dto.FuelType.Value.ToString());
            if (fuelTypeLookup == null)
            {
                throw new Exception("FuelType is invalid");
            }

            if (!dto.ManufactureCountryId.HasValue)
            {
                throw new Exception("ManufactureCountryId is required");
            }

            var manufactureCountryLookup = await _unitOfWork.LookupDetails
                .GetByMasterAndDetailAsync("COUNTRY", dto.ManufactureCountryId.Value.ToString());
            if (manufactureCountryLookup == null)
            {
                throw new Exception("ManufactureCountryId is invalid");
            }

            var model = await _unitOfWork.CarModels.GetByIdAsync(dto.ModelId);
            if (model == null)
            {
                throw new Exception("CarModel not found");
            }

            var type = await _unitOfWork.CarTypes.GetCarTypeById(dto.TypeId);
            if (type == null)
            {
                throw new Exception("CarType not found");
            }

            var branch = await _unitOfWork.Branches.GetByIdAsync(dto.BranchId);
            if (branch == null)
            {
                throw new Exception("Branch not found");
            }

            if (dto.IsAvailable == null)
            {
                dto.IsAvailable = existing.IsAvailable;
            }

            _mapper.Map(dto, existing);
            await _unitOfWork.Cars.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Cars.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Car not found");
            }

            var hasQuotations = (await _unitOfWork.Quotations.GetAllAsync())
                .Any(q => q.CarId == id && q.IsAvailable);
            if (hasQuotations)
            {
                throw new Exception("Cannot delete car because it is referenced by quotations");
            }

            try
            {
                await _unitOfWork.Cars.DeleteAsync(existing);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Cannot delete car because it is referenced by related data");
            }
        }

        public async Task SetAvailabilityAsync(int id, bool isAvailable)
        {
            var existing = await _unitOfWork.Cars.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Car not found");
            }

            existing.IsAvailable = isAvailable;
            await _unitOfWork.Cars.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BulkDeleteCarsResponseDto> BulkDeleteAsync(List<int> carIds)
        {
            var response = new BulkDeleteCarsResponseDto();
            foreach (var id in carIds)
            {
                try
                {
                    await DeleteAsync(id);
                    response.DeletedCount++;
                }
                catch
                {
                    response.FailedIds.Add(id);
                }
            }

            return response;
        }

        public async Task<List<CarResponseDto>> FilterAsync(int? modelId = null, int? typeId = null, bool? isAvailable = null)
        {
            var cars = await _unitOfWork.Cars.FilterAsync(modelId, typeId, isAvailable);
            return _mapper.Map<List<CarResponseDto>>(cars);
        }

        private CreateCarRequestDto BuildBaseCarCreateRequest(CreateCarWithDetailsRequestDto dto)
        {
            return new CreateCarRequestDto
            {
                NameAr = dto.NameAr,
                NameEn = dto.NameEn,
                ModelId = dto.ModelId,
                TypeId = dto.TypeId,
                BranchId = dto.BranchId,
                Year = dto.Year,
                Mileage = dto.Mileage,
                Vat = dto.Vat,
                ConditionId = dto.ConditionId,
                SeatingCapacity = dto.SeatingCapacity,
                WeelSizeInch = dto.WeelSizeInch,
                FuelTankCapacityLiter = dto.FuelTankCapacityLiter,
                TrimLevel = dto.TrimLevel,
                VehicleClass = dto.VehicleClass,
                PlateNumberAr = dto.PlateNumberAr,
                PlateNumberEn = dto.PlateNumberEn,
                TransmisionType = dto.TransmisionType,
                Drivetrain = dto.Drivetrain,
                Cylenders = dto.Cylenders,
                FuelType = dto.FuelType,
                ManufactureCountryId = dto.ManufactureCountryId,
                EnginNumber = dto.EnginNumber,
                DescriptionAr = dto.DescriptionAr,
                DescriptionEn = dto.DescriptionEn
            };
        }

        private CreateWithDetailsPayload BuildCreateWithDetailsPayload(CreateCarWithDetailsRequestDto dto)
        {
            var features = ParseJson<List<CreateCarWithDetailsFeatureItemDto>>(dto.FeaturesJson) ?? new();
            var carColors = ParseJson<List<CreateCarWithDetailsColorItemDto>>(dto.CarColorsJson) ?? new();
            var extraDetails = ParseJson<List<CreateCarWithDetailsExtraDetailItemDto>>(dto.ExtraDetailsJson) ?? new();
            var galleryMeta = ParseJson<List<CreateCarWithDetailsGalleryImageMetaItemDto>>(dto.GalleryImagesMetaJson) ?? new();
            var carColorImagesMeta = ParseJson<List<CreateCarWithDetailsCarColorImageMetaItemDto>>(dto.CarColorImagesMetaJson) ?? new();

            var carColorFiles = dto.CarColorImageFiles?.Where(f => f != null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var galleryFiles = dto.GalleryImageFiles?.Where(f => f != null && f.Length > 0).ToList() ?? new List<IFormFile>();

            return new CreateWithDetailsPayload
            {
                Features = features,
                CarColors = carColors,
                ExtraDetails = extraDetails,
                GalleryMeta = galleryMeta,
                GalleryFiles = galleryFiles,
                UserName = _currentUserService.UserName,
                CarColorFileByColorId = BuildCarColorFileMap(carColorFiles, carColorImagesMeta)
            };
        }

        private Dictionary<int, IFormFile> BuildCarColorFileMap(
            IReadOnlyList<IFormFile> uploadedCarColorFiles,
            IReadOnlyList<CreateCarWithDetailsCarColorImageMetaItemDto> carColorImagesMeta)
        {
            var fileByColorId = new Dictionary<int, IFormFile>();

            foreach (var file in uploadedCarColorFiles)
            {
                var meta = carColorImagesMeta.FirstOrDefault(m =>
                    m.ColorId > 0 &&
                    !string.IsNullOrWhiteSpace(m.FileName) &&
                    string.Equals(m.FileName, file.FileName, StringComparison.OrdinalIgnoreCase));

                if (meta?.ColorId > 0)
                {
                    fileByColorId[meta.ColorId] = file;
                }
            }

            return fileByColorId;
        }

        private async Task SaveFeatureAssignmentsAsync(
            int carId,
            IReadOnlyList<CreateCarWithDetailsFeatureItemDto> features,
            string? userName)
        {
            var uniqueFeatures = features
                .Where(x => x.FeatureId > 0)
                .GroupBy(x => x.FeatureId)
                .Select(g => g.Last());

            foreach (var feature in uniqueFeatures)
            {
                await _unitOfWork.CarFeatures.AddCarFeatureAssignmentAsync(new CarFeature
                {
                    CarId = carId,
                    FeatureId = feature.FeatureId,
                    IsAvailable = feature.IsAvailable,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userName
                });
            }
        }

        private async Task SaveCarColorsAsync(
            int carId,
            IReadOnlyList<CreateCarWithDetailsColorItemDto> carColors,
            IReadOnlyDictionary<int, IFormFile> carColorFileByColorId,
            decimal carVat)
        {
            var uniqueColors = carColors
                .Where(x => x.ColorId > 0)
                .GroupBy(x => x.ColorId)
                .Select(g => g.Last());

            foreach (var color in uniqueColors)
            {
                var colorImageUrl = color.ColorImageUrl;
                if (carColorFileByColorId.TryGetValue(color.ColorId, out var colorImageFile))
                {
                    var savedName = await SaveCarImageAsync(colorImageFile);
                    colorImageUrl = $"/uploads/cars/{savedName}";
                }

                var carColor = new CarColor
                {
                    CarId = carId,
                    ColorId = color.ColorId,
                    StockQuantity = color.StockQuantity,
                    ColorImageUrl = colorImageUrl,
                    IsAvailable = color.IsAvailable,
                    CreatedAt = DateTime.UtcNow
                };

                carColor.ApplyPricing(
                    color.PricingPerColor ?? 0m,
                    color.PricePefore ?? color.PricingPerColor ?? 0m,
                    carVat,
                    color.Discount ?? 0m,
                    color.DiscountType ?? CarColor.DiscountTypePercentage);

                await _unitOfWork.CarCarColors.CreateAsync(carColor);
            }
        }

        private async Task SaveExtraDetailsAsync(
            int carId,
            IReadOnlyList<CreateCarWithDetailsExtraDetailItemDto> extraDetails,
            string? userName)
        {
            foreach (var detail in extraDetails)
            {
                var extraTypeLookup = await _unitOfWork.LookupDetails
                    .GetByMasterAndDetailAsync("EXTRA_TYPE", detail.CarExtraDetailsType.ToString());
                if (extraTypeLookup == null)
                {
                    throw new Exception("CarExtraDetailsType is invalid");
                }

                await _unitOfWork.CarExtraDetails.CreateAsync(new CarExtraDetails
                {
                    CarId = carId,
                    NameAr = detail.NameAr,
                    NameEn = detail.NameEn,
                    DescriptionAr = detail.DescriptionAr,
                    DescriptionEn = detail.DescriptionEn,
                    CarExtraDetailsType = detail.CarExtraDetailsType,
                    IsAvailable = detail.IsAvailable,
                    CreatedBy = userName,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        private async Task SaveGalleryImagesAsync(
            int carId,
            IReadOnlyList<CreateCarWithDetailsGalleryImageMetaItemDto> galleryMeta,
            IReadOnlyList<IFormFile> uploadedFiles,
            string? userName)
        {
            if (uploadedFiles.Count == 0)
            {
                return;
            }

            var firstMarkedPrimaryExists = galleryMeta.Any(m => m.IsPrimary);

            for (var i = 0; i < uploadedFiles.Count; i++)
            {
                var file = uploadedFiles[i];
                var savedName = await SaveCarImageAsync(file);
                var imageUrl = $"/uploads/cars/{savedName}";
                var meta = galleryMeta.FirstOrDefault(m =>
                    !string.IsNullOrWhiteSpace(m.FileName) &&
                    string.Equals(m.FileName, file.FileName, StringComparison.OrdinalIgnoreCase));

                await _unitOfWork.CarGalleryImages.AddImageAsync(new CarGalleryImage
                {
                    CarId = carId,
                    ImageUrl = imageUrl,
                    ImageType = meta?.ImageType,
                    IsPrimary = meta?.IsPrimary ?? (!firstMarkedPrimaryExists && i == 0),
                    CreatedBy = userName,
                    CreatedAt = DateTime.UtcNow,
                    IsAvailable = true
                });
            }
        }

        private async Task<string> SaveCarImageAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "cars");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(
                CultureInfo.InvariantCulture,
                $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        private static T? ParseJson<T>(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private sealed class CreateWithDetailsPayload
        {
            public List<CreateCarWithDetailsFeatureItemDto> Features { get; init; } = new();
            public List<CreateCarWithDetailsColorItemDto> CarColors { get; init; } = new();
            public List<CreateCarWithDetailsExtraDetailItemDto> ExtraDetails { get; init; } = new();
            public List<CreateCarWithDetailsGalleryImageMetaItemDto> GalleryMeta { get; init; } = new();
            public List<IFormFile> GalleryFiles { get; init; } = new();
            public Dictionary<int, IFormFile> CarColorFileByColorId { get; init; } = new();
            public string? UserName { get; init; }
        }
    }
}
