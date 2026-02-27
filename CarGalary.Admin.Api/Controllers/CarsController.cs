using CarGalary.Admin.Api.Security;
using CarGalary.Admin.Api.Hubs;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.CarGalleryImage.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ICarGalleryImageService _carImageService;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<CarHub> _hubContext;

        public CarsController(
            ICarService carService,
            ICarGalleryImageService carImageService,
            IWebHostEnvironment environment,
            IHubContext<CarHub> hubContext)
        {
            _carService = carService;
            _carImageService = carImageService;
            _environment = environment;
            _hubContext = hubContext;
        }

        [HttpGet]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cars = await _carService.GetAllAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var car = await _carService.GetByIdAsync(id);
                if (car == null)
                {
                    return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
                }
                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("filter")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> Filter([FromQuery] int? modelId, [FromQuery] int? typeId, [FromQuery] bool? isAvailable)
        {
            try
            {
                var cars = await _carService.FilterAsync(modelId, typeId, isAvailable);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("by-model/{modelId:int}")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetCarsByModel(int modelId)
        {
            try
            {
                var cars = await _carService.FilterAsync(modelId, null, null);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost]
        [PermissionAuthorize("cars.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarRequestDto dto,
            [FromServices] IValidator<CreateCarRequestDto> validator)
        {
            try
            {
                var validationResult = validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var created = await _carService.CreateAsync(dto);
                await _hubContext.Clients.All.SendAsync("carCreated", created);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return BadRequest(new ApiErrorResponse("Model ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return BadRequest(new ApiErrorResponse("Type ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "Branch not found")
            {
                return BadRequest(new ApiErrorResponse("Branch ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarRequestDto dto,
            [FromServices] IValidator<UpdateCarRequestDto> validator)
        {
            try
            {
                var existing = await _carService.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
                }

                var validationResult = validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                await _carService.UpdateAsync(id, dto);
                var updated = await _carService.GetByIdAsync(id);
                if (updated != null)
                {
                    await _hubContext.Clients.All.SendAsync("carUpdated", updated);
                }
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return BadRequest(new ApiErrorResponse("Model ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return BadRequest(new ApiErrorResponse("Type ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "Branch not found")
            {
                return BadRequest(new ApiErrorResponse("Branch ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("cars.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _carService.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
                }

                await _carService.DeleteAsync(id);
                await _hubContext.Clients.All.SendAsync("carDeleted", new
                {
                    existing.Id,
                    existing.NameEn,
                    existing.NameAr
                });
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex) when (ex.Message == "Cannot delete car because it is referenced by quotations")
            {
                return BadRequest(new ApiErrorResponse("Cannot delete this car because it is linked to quotation records.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "Cannot delete car because it is referenced by related data")
            {
                return BadRequest(new ApiErrorResponse("Cannot delete this car because it is linked to related records.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("{id:int}/availability")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] UpdateCarAvailabilityRequestDto dto)
        {
            try
            {
                var existing = await _carService.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
                }

                await _carService.SetAvailabilityAsync(id, dto.IsAvailable);
                var updated = await _carService.GetByIdAsync(id);
                if (updated != null)
                {
                    await _hubContext.Clients.All.SendAsync("carUpdated", updated);
                }
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("{id:int}/copy")]
        [PermissionAuthorize("cars.create")]
        public async Task<IActionResult> Copy(int id)
        {
            try
            {
                var created = await _carService.CopyAsync(id);
                await _hubContext.Clients.All.SendAsync("carCreated", created);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound(new ApiErrorResponse("Car not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("cars.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteCarsRequest request)
        {
            try
            {
                if (request.CarIds == null || !request.CarIds.Any())
                {
                    return BadRequest(new ApiErrorResponse("Car IDs are required", StatusCodes.Status400BadRequest));
                }

                var result = await _carService.BulkDeleteAsync(request.CarIds);
                var deletedIds = request.CarIds
                    .Except(result.FailedIds ?? new List<int>())
                    .Distinct()
                    .ToList();

                if (deletedIds.Count > 0)
                {
                    foreach (var deletedId in deletedIds)
                    {
                        await _hubContext.Clients.All.SendAsync("carDeleted", new
                        {
                            Id = deletedId,
                            NameEn = string.Empty,
                            NameAr = string.Empty
                        });
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("save-all")]
        [PermissionAuthorize("cars.create")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
        [RequestSizeLimit(104_857_600)]
        public async Task<IActionResult> SaveAll(
            [FromForm] CreateCarWithDetailsRequestDto dto,
            [FromServices] IValidator<CreateCarRequestDto> validator,
            [FromServices] IValidator<CreateCarWithDetailsFeatureItemDto> featureItemValidator,
            [FromServices] IValidator<CreateCarWithDetailsColorItemDto> colorItemValidator,
            [FromServices] IValidator<CreateCarWithDetailsExtraDetailItemDto> extraDetailItemValidator,
            [FromServices] IValidator<CreateCarWithDetailsGalleryImageMetaItemDto> galleryMetaItemValidator,
            [FromServices] IValidator<CreateCarWithDetailsCarColorImageMetaItemDto> carColorImageMetaItemValidator)
        {
            try
            {
                var baseCarDto = new CreateCarRequestDto
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

                var validationResult = validator.Validate(baseCarDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var features = ParseJson<List<CreateCarWithDetailsFeatureItemDto>>(dto.FeaturesJson) ?? new();
                var carColors = ParseJson<List<CreateCarWithDetailsColorItemDto>>(dto.CarColorsJson) ?? new();
                var extraDetails = ParseJson<List<CreateCarWithDetailsExtraDetailItemDto>>(dto.ExtraDetailsJson) ?? new();
                var galleryMeta = ParseJson<List<CreateCarWithDetailsGalleryImageMetaItemDto>>(dto.GalleryImagesMetaJson) ?? new();
                var carColorImagesMeta = ParseJson<List<CreateCarWithDetailsCarColorImageMetaItemDto>>(dto.CarColorImagesMetaJson) ?? new();

                var nestedErrors = new List<string>();
                nestedErrors.AddRange(ValidateItems(features, featureItemValidator, "features"));
                nestedErrors.AddRange(ValidateItems(carColors, colorItemValidator, "carColors"));
                nestedErrors.AddRange(ValidateItems(extraDetails, extraDetailItemValidator, "extraDetails"));
                nestedErrors.AddRange(ValidateItems(galleryMeta, galleryMetaItemValidator, "galleryImagesMeta"));
                nestedErrors.AddRange(ValidateItems(carColorImagesMeta, carColorImageMetaItemValidator, "carColorImagesMeta"));

                if (nestedErrors.Count > 0)
                {
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, nestedErrors));
                }

                var createdCar = await _carService.CreateWithDetailsAsync(dto);
                await _hubContext.Clients.All.SendAsync("carCreated", createdCar);
                return Ok(createdCar);
            }
            catch (JsonException ex)
            {
                return BadRequest(new ApiErrorResponse($"Invalid JSON payload: {ex.Message}", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return BadRequest(new ApiErrorResponse("Model ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return BadRequest(new ApiErrorResponse("Type ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "Branch not found")
            {
                return BadRequest(new ApiErrorResponse("Branch ID is not valid", StatusCodes.Status400BadRequest));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ApiErrorResponse($"Save failed: {ex.GetBaseException().Message}", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        // Car Images Endpoints
        [HttpGet("{carId:int}/images")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetCarImages(int carId)
        {
            try
            {
                var images = await _carImageService.GetByCarIdAsync(carId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("{carId:int}/images")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> UploadCarImage(int carId, IFormFile imageFile, [FromQuery] bool isPrimary = false, [FromQuery] int? imageType = null)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest(new ApiErrorResponse("Image file is required", StatusCodes.Status400BadRequest));
                }

                var fileName = await SaveCarImageAsync(imageFile);
                var imageUrl = $"/uploads/cars/{fileName}";

                var dto = new CreateCarGalleryImageRequestDto
                {
                    CarId = carId,
                    ImageUrl = imageUrl,
                    IsPrimary = isPrimary,
                    ImageType = imageType
                };

                var created = await _carImageService.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("images/{imageId:int}")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> DeleteCarImage(int imageId)
        {
            try
            {
                await _carImageService.DeleteAsync(imageId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("images/{imageId:int}/primary")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> SetPrimaryImage(int imageId, [FromBody] UpdateCarGalleryImageRequestDto dto)
        {
            try
            {
                await _carImageService.UpdateAsync(imageId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
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

        private static List<string> ValidateItems<T>(
            IReadOnlyList<T> items,
            IValidator<T> validator,
            string fieldName)
        {
            var errors = new List<string>();
            for (var i = 0; i < items.Count; i++)
            {
                var result = validator.Validate(items[i]);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors.Select(e => $"{fieldName}[{i}].{e.ErrorMessage}"));
                }
            }

            return errors;
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

    }
}
