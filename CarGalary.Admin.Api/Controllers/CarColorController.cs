using CarGalary.Application.Dtos.CarCarColor.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarColorController : ControllerBase
    {
        private readonly ICarCarColorService _service;
        private readonly IWebHostEnvironment _environment;

        public CarColorController(ICarCarColorService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{carId:int}/{colorId:int}")]
        public async Task<IActionResult> GetById(int carId, int colorId)
        {
            var item = await _service.GetByIdAsync(carId, colorId);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("by-car/{carId:int}")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            var items = await _service.GetByCarIdAsync(carId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] CreateCarCarColorRequestDto dto,
            [FromServices] IValidator<CreateCarCarColorRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                if (dto.ColorImageFile != null)
                {
                    dto.ColorImageUrl = await SaveCarCarColorImageAsync(dto.ColorImageFile);
                }

                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new[] { "CarId is not valid" });
            }
            catch (Exception ex) when (ex.Message == "CarColor not found")
            {
                return BadRequest(new[] { "ColorId is not valid" });
            }
            catch (Exception ex) when (ex.Message == "CarCarColor already exists")
            {
                return BadRequest(new[] { "CarId + ColorId already exists" });
            }
        }

        [HttpPut("{carId:int}/{colorId:int}")]
        public async Task<IActionResult> Update(
            int carId,
            int colorId,
            [FromForm] UpdateCarCarColorRequestDto dto,
            [FromServices] IValidator<UpdateCarCarColorRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(carId, colorId);
            if (existing == null) return NotFound();

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                if (dto.ColorImageFile != null)
                {
                    DeleteCarCarColorImageIfExists(existing.ColorImageUrl);
                    dto.ColorImageUrl = await SaveCarCarColorImageAsync(dto.ColorImageFile);
                }

                await _service.UpdateAsync(carId, colorId, dto); // Task only (no return body)
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarCarColor not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{carId:int}/{colorId:int}")]
        public async Task<IActionResult> Delete(int carId, int colorId)
        {
            var existing = await _service.GetByIdAsync(carId, colorId);
            if (existing == null) return NotFound();

            try
            {
                DeleteCarCarColorImageIfExists(existing.ColorImageUrl);
                await _service.DeleteAsync(carId, colorId); // Task only (no return body)
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarCarColor not found")
            {
                return NotFound();
            }
        }

        private async Task<string> SaveCarCarColorImageAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "car-colors");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(
                CultureInfo.InvariantCulture,
                $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/car-colors/{fileName}";
        }

        private void DeleteCarCarColorImageIfExists(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "car-colors");

            string? relativePath = null;
            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var absoluteUri))
            {
                relativePath = absoluteUri.AbsolutePath.TrimStart('/');
            }
            else
            {
                relativePath = imageUrl.TrimStart('/');
            }

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return;
            }

            var fullPath = Path.GetFullPath(Path.Combine(rootPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            var uploadFolderFullPath = Path.GetFullPath(uploadFolder);

            if (!fullPath.StartsWith(uploadFolderFullPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
