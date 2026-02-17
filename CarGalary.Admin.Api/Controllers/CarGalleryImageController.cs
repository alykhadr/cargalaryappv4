using CarGalary.Application.Dtos.CarGalleryImage.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarGalleryImageController : ControllerBase
    {
        private readonly ICarGalleryImageService _service;
        private readonly IWebHostEnvironment _environment;

        public CarGalleryImageController(ICarGalleryImageService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _service.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        [HttpGet("by-car/{carId:int}")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            var images = await _service.GetByCarIdAsync(carId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] CreateCarGalleryImageRequestDto dto,
            [FromServices] IValidator<CreateCarGalleryImageRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                dto.ImageUrl = await SaveImageAsync(dto.ImageFile);
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new[] { "CarId is not valid" });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateCarGalleryImageRequestDto dto,
            [FromServices] IValidator<UpdateCarGalleryImageRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                DeleteImageIfExists(existing.ImageUrl);
                dto.ImageUrl = await SaveImageAsync(dto.ImageFile);
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarGalleryImage not found")
            {
                return NotFound();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new[] { "CarId is not valid" });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            DeleteImageIfExists(existing.ImageUrl);

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarGalleryImage not found")
            {
                return NotFound();
            }
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "car-gallery-images");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(
                CultureInfo.InvariantCulture,
                $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/car-gallery-images/{fileName}";
        }

        private void DeleteImageIfExists(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "car-gallery-images");

            string? relativePath;
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
