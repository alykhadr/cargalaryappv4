using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Package.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PackagesController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string PackageNotFoundCode = "1334";
        private const string PackageIdsRequiredCode = "1231";

        private readonly IPackageService _service;
        private readonly IWebHostEnvironment _env;

        public PackagesController(IPackageService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        [PermissionAuthorize("packages.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("packages.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new ApiErrorResponse(PackageNotFoundCode, StatusCodes.Status404NotFound));
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("packages.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreatePackageRequestDto dto,
            [FromServices] IValidator<CreatePackageRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            if (dto.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "packages");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.ImageUrl = $"/uploads/packages/{fileName}";
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("packages.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdatePackageRequestDto dto,
            [FromServices] IValidator<UpdatePackageRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(PackageNotFoundCode, StatusCodes.Status404NotFound));

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "packages");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.ImageUrl = $"/uploads/packages/{fileName}";
            }
            else
            {
                dto.ImageUrl = existing.ImageUrl;
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Package not found")
            {
                return NotFound(new ApiErrorResponse(PackageNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("packages.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(PackageNotFoundCode, StatusCodes.Status404NotFound));

            if (!string.IsNullOrEmpty(existing.ImageUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
            }

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Package not found")
            {
                return NotFound(new ApiErrorResponse(PackageNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("packages.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeletePackagesRequest request)
        {
            if (request.PackageIds == null || !request.PackageIds.Any())
            {
                return BadRequest(new ApiErrorResponse(PackageIdsRequiredCode, StatusCodes.Status400BadRequest));
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var packageId in request.PackageIds)
            {
                try
                {
                    var existing = await _service.GetByIdAsync(packageId);
                    if (existing != null && !string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var imagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                    }
                    await _service.DeleteAsync(packageId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(packageId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}
