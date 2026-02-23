using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Offer.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _service;
        private readonly IWebHostEnvironment _env;

        public OffersController(IOfferService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        [PermissionAuthorize("offers.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("offers.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("offers.create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateOfferRequestDto dto,
            [FromServices] IValidator<CreateOfferRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            if (dto.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "offers");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.OfferImageUrl = $"/uploads/offers/{fileName}";
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("offers.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] UpdateOfferRequestDto dto,
            [FromServices] IValidator<UpdateOfferRequestDto> validator)
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
                if (!string.IsNullOrEmpty(existing.OfferImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existing.OfferImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "offers");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                dto.OfferImageUrl = $"/uploads/offers/{fileName}";
            }
            else
            {
                dto.OfferImageUrl = existing.OfferImageUrl;
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Offer not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("offers.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (!string.IsNullOrEmpty(existing.OfferImageUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, existing.OfferImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
            }

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Offer not found")
            {
                return NotFound();
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("offers.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteOfferRequest request)
        {
            if (request.OfferIds == null || !request.OfferIds.Any())
            {
                return BadRequest("Offer IDs are required");
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var offerId in request.OfferIds)
            {
                try
                {
                    var existing = await _service.GetByIdAsync(offerId);
                    if (existing != null && !string.IsNullOrEmpty(existing.OfferImageUrl))
                    {
                        var imagePath = Path.Combine(_env.WebRootPath, existing.OfferImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                    }
                    await _service.DeleteAsync(offerId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(offerId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}