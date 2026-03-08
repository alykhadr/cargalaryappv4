using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.FAQ.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FAQController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string FaqNotFoundCode = "1332";
        private const string FaqIdsRequiredCode = "1215";

        private readonly IFAQService _service;

        public FAQController(IFAQService service)
        {
            _service = service;
        }

        [HttpGet]
        [PermissionAuthorize("faq.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("faq.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new ApiErrorResponse(FaqNotFoundCode, StatusCodes.Status404NotFound));
            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("faq.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateFAQRequestDto dto,
            [FromServices] IValidator<CreateFAQRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("faq.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateFAQRequestDto dto,
            [FromServices] IValidator<UpdateFAQRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(FaqNotFoundCode, StatusCodes.Status404NotFound));

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "FAQ not found")
            {
                return NotFound(new ApiErrorResponse(FaqNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("faq.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(FaqNotFoundCode, StatusCodes.Status404NotFound));

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "FAQ not found")
            {
                return NotFound(new ApiErrorResponse(FaqNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("faq.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteFAQRequest request)
        {
            if (request.FaqIds == null || !request.FaqIds.Any())
            {
                return BadRequest(new ApiErrorResponse(FaqIdsRequiredCode, StatusCodes.Status400BadRequest));
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var faqId in request.FaqIds)
            {
                try
                {
                    await _service.DeleteAsync(faqId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(faqId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}
