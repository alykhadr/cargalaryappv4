using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CarGalary.Admin.Api.Security;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IWebHostEnvironment _environment;

        public BrandsController(IBrandService brandService, IWebHostEnvironment environment)
        {
            _brandService = brandService;
            _environment = environment;
        }

        [HttpGet]
        [PermissionAuthorize("brands.view")]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("brands.view")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(id);
                if (brand == null)
                {
                    return NotFound(new ApiErrorResponse("Brand not found", StatusCodes.Status404NotFound));
                }
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("{brandId:int}/models")]
        [PermissionAuthorize("brands.view")]
        public async Task<IActionResult> GetCarModelsByBrand(int brandId)
        {
            var models = await _brandService.GetCarModelsByBrandAsync(brandId);
            return Ok(models);
        }

        [HttpPost]
        [PermissionAuthorize("brands.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateBrandRequestDto createBrandRequestDto,
            [FromServices] IValidator<CreateBrandRequestDto> validator)
        {
            try
            {
                var validationResult = validator.Validate(createBrandRequestDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                if (createBrandRequestDto.ImageFile != null)
                {
                    createBrandRequestDto.ImageUrl = await SaveBrandImageAsync(createBrandRequestDto.ImageFile);
                }

                var created = await _brandService.CreateAsync(createBrandRequestDto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("brands.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateBrandRequestDto updateBrandRequestDto,
            [FromServices] IValidator<UpdateBrandRequestDto> validator)
        {
            try
            {
                var existingBrand = await _brandService.GetByIdAsync(id);
                if (existingBrand == null)
                {
                    return NotFound(new ApiErrorResponse("Brand not found", StatusCodes.Status404NotFound));
                }

                var validationResult = validator.Validate(updateBrandRequestDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                if (updateBrandRequestDto.ImageFile != null)
                {
                    DeleteBrandImageIfExists(existingBrand.ImageUrl);
                    updateBrandRequestDto.ImageUrl = await SaveBrandImageAsync(updateBrandRequestDto.ImageFile);
                }
                else
                {
                    updateBrandRequestDto.ImageUrl = existingBrand.ImageUrl;
                }

                await _brandService.UpdateAsync(id, updateBrandRequestDto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound(new ApiErrorResponse("Brand not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("brands.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingBrand = await _brandService.GetByIdAsync(id);
                if (existingBrand == null)
                {
                    return NotFound(new ApiErrorResponse("Brand not found", StatusCodes.Status404NotFound));
                }

                await _brandService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound(new ApiErrorResponse("Brand not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("brands.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteBrandsRequest request)
        {
            try
            {
                if (request.BrandIds == null || !request.BrandIds.Any())
                {
                    return BadRequest(new ApiErrorResponse("Brand IDs are required", StatusCodes.Status400BadRequest));
                }

                var deletedCount = 0;
                var failedIds = new List<int>();

                foreach (var brandId in request.BrandIds)
                {
                    try
                    {
                        await _brandService.DeleteAsync(brandId);
                        deletedCount++;
                    }
                    catch
                    {
                        failedIds.Add(brandId);
                    }
                }

                return Ok(new { deletedCount, failedIds });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
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
