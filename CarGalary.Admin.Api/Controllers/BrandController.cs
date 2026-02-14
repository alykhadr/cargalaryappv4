using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IWebHostEnvironment _environment;

        public BrandController(IBrandService brandService, IWebHostEnvironment environment)
        {
            _brandService = brandService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] CreateBrandRequestDto createBrandRequestDto,
            [FromServices] IValidator<CreateBrandRequestDto> validator)
        {
            var validationResult = validator.Validate(createBrandRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (createBrandRequestDto.ImageFile != null)
            {
                createBrandRequestDto.ImageUrl = await SaveBrandImageAsync(createBrandRequestDto.ImageFile);
            }

            var created = await _brandService.CreateAsync(createBrandRequestDto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateBrandRequestDto updateBrandRequestDto,
            [FromServices] IValidator<UpdateBrandRequestDto> validator)
        {
            var existingBrand = await _brandService.GetByIdAsync(id);
            if (existingBrand == null)
            {
                return NotFound();
            }

            var validationResult = validator.Validate(updateBrandRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (updateBrandRequestDto.ImageFile != null)
            {
                DeleteBrandImageIfExists(existingBrand.ImageUrl);
                updateBrandRequestDto.ImageUrl = await SaveBrandImageAsync(updateBrandRequestDto.ImageFile);
            }

            try
            {
                await _brandService.UpdateAsync(id, updateBrandRequestDto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound();
            }
        }

        private void DeleteBrandImageIfExists(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "brands");

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

        private async Task<string> SaveBrandImageAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "brands");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(
                CultureInfo.InvariantCulture,
                $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/brands/{fileName}";
        }
    }
}
