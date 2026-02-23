using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.MemberService.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberServiceController : ControllerBase
    {
        private readonly IMemberServiceService _service;
        private readonly IWebHostEnvironment _env;

        public MemberServiceController(IMemberServiceService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        [PermissionAuthorize("memberservices.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("memberservices.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("memberservices.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateMemberServiceRequestDto dto,
            [FromServices] IValidator<CreateMemberServiceRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "member-services");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.ImageUrl = $"/uploads/member-services/{fileName}";
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("memberservices.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateMemberServiceRequestDto dto,
            [FromServices] IValidator<UpdateMemberServiceRequestDto> validator)
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
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "member-services");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.ImageUrl = $"/uploads/member-services/{fileName}";
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
            catch (Exception ex) when (ex.Message == "MemberService not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("memberservices.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

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
            catch (Exception ex) when (ex.Message == "MemberService not found")
            {
                return NotFound();
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("memberservices.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteMemberServiceRequest request)
        {
            if (request.MemberServiceIds == null || !request.MemberServiceIds.Any())
            {
                return BadRequest("MemberService IDs are required");
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var serviceId in request.MemberServiceIds)
            {
                try
                {
                    var existing = await _service.GetByIdAsync(serviceId);
                    if (existing != null && !string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var imagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                    }
                    await _service.DeleteAsync(serviceId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(serviceId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}