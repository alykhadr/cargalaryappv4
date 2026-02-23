using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.CompanyInformation.Command;
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
    public class CompanyInformationsController : ControllerBase
    {
        private readonly ICompanyInformationService _service;
        private readonly IWebHostEnvironment _environment;

        public CompanyInformationsController(ICompanyInformationService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        [PermissionAuthorize("companyinfo.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("companyinfo.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("companyinfo.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateCompanyInformationRequestDto dto,
            [FromServices] IValidator<CreateCompanyInformationRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.LogoFile != null)
            {
                dto.LogoUrl = await SaveLogoAsync(dto.LogoFile);
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("companyinfo.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateCompanyInformationRequestDto dto,
            [FromServices] IValidator<UpdateCompanyInformationRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (dto.LogoFile == null)
            {
                dto.LogoUrl = existing.LogoUrl;
            }

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.LogoFile != null)
            {
                DeleteLogoIfExists(existing.LogoUrl);
                dto.LogoUrl = await SaveLogoAsync(dto.LogoFile);
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CompanyInformation not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("companyinfo.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            DeleteLogoIfExists(existing.LogoUrl);

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CompanyInformation not found")
            {
                return NotFound();
            }
        }

        private async Task<string> SaveLogoAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "company-information");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(CultureInfo.InvariantCulture, $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/company-information/{fileName}";
        }

        private void DeleteLogoIfExists(string? logoUrl)
        {
            if (string.IsNullOrWhiteSpace(logoUrl))
            {
                return;
            }

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "company-information");

            string? relativePath;
            if (Uri.TryCreate(logoUrl, UriKind.Absolute, out var absoluteUri))
            {
                relativePath = absoluteUri.AbsolutePath.TrimStart('/');
            }
            else
            {
                relativePath = logoUrl.TrimStart('/');
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
