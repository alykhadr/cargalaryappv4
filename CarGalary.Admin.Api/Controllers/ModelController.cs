
using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.CarModel.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelController : ControllerBase
    {
        private readonly ICarModelService _service;
        private readonly IWebHostEnvironment _environment;

        public ModelController(ICarModelService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        [PermissionAuthorize("models.view")]
        public async Task<IActionResult> GetAll()
        {
            var models = await _service.GetAllAsync();
            return Ok(models);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("models.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _service.GetByIdAsync(id);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpPost]
        [PermissionAuthorize("models.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateCarModelRequestDto dto,
            [FromServices] IValidator<CreateCarModelRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                dto.ImageUrl = await SaveModelImageAsync(dto.ImageFile);
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return BadRequest(new[] { "BrandId is not valid" });
            }
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("models.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateCarModelRequestDto dto,
            [FromServices] IValidator<UpdateCarModelRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                DeleteModelImageIfExists(existing.ImageUrl);
                dto.ImageUrl = await SaveModelImageAsync(dto.ImageFile);
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return NotFound();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return BadRequest(new[] { "BrandId is not valid" });
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("models.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            DeleteModelImageIfExists(existing.ImageUrl);

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return NotFound();
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("models.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteCarModelRequestDto dto)
        {
            var result = await _service.BulkDeleteAsync(dto.ModelIds);
            return Ok(result);
        }

        private async Task<string> SaveModelImageAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "models");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(
                CultureInfo.InvariantCulture,
                $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/models/{fileName}";
        }

        private void DeleteModelImageIfExists(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "models");

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
