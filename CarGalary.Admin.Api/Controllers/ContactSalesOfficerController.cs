using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
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
    public class ContactSalesOfficerController : ControllerBase
    {
        private readonly IContactSalesOfficerService _service;
        private readonly IWebHostEnvironment _environment;

        public ContactSalesOfficerController(IContactSalesOfficerService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        [PermissionAuthorize("contactsales.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("contactsales.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("contactsales.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateContactSalesOfficerRequestDto dto,
            [FromServices] IValidator<CreateContactSalesOfficerRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.IconFile != null)
            {
                dto.ContactIconUrl = await SaveIconAsync(dto.IconFile);
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("contactsales.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateContactSalesOfficerRequestDto dto,
            [FromServices] IValidator<UpdateContactSalesOfficerRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.IconFile != null)
            {
                DeleteIconIfExists(existing.ContactIconUrl);
                dto.ContactIconUrl = await SaveIconAsync(dto.IconFile);
            }
            else
            {
                dto.ContactIconUrl = existing.ContactIconUrl;
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "ContactSalesOfficer not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("contactsales.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "ContactSalesOfficer not found")
            {
                return NotFound();
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("contactsales.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteContactSalesRequest request)
        {
            if (request.ContactIds == null || !request.ContactIds.Any())
            {
                return BadRequest("Contact IDs are required");
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var contactId in request.ContactIds)
            {
                try
                {
                    await _service.DeleteAsync(contactId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(contactId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }

        private void DeleteIconIfExists(string? iconUrl)
        {
            if (string.IsNullOrWhiteSpace(iconUrl)) return;

            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", "contact-icons");

            string? relativePath = null;
            if (Uri.TryCreate(iconUrl, UriKind.Absolute, out var absoluteUri))
            {
                relativePath = absoluteUri.AbsolutePath.TrimStart('/');
            }
            else
            {
                relativePath = iconUrl.TrimStart('/');
            }

            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var fullPath = Path.GetFullPath(Path.Combine(rootPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            var uploadFolderFullPath = Path.GetFullPath(uploadFolder);

            if (!fullPath.StartsWith(uploadFolderFullPath, StringComparison.OrdinalIgnoreCase)) return;

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private async Task<string> SaveIconAsync(IFormFile file)
        {
            var rootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads", "contact-icons");
            Directory.CreateDirectory(uploadPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = string.Create(CultureInfo.InvariantCulture, $"{Guid.NewGuid():N}{extension}");
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{Request.Scheme}://{Request.Host}/uploads/contact-icons/{fileName}";
        }
    }
}
