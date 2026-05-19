using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.PrivacyPolicy.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivacyPoliciesController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string PrivacyPolicyNotFoundCode = "1335";
        private const string PrivacyPolicyIdsRequiredCode = "1232";

        private readonly IPrivacyPolicyService _service;

        public PrivacyPoliciesController(IPrivacyPolicyService service)
        {
            _service = service;
        }

        [HttpGet]
        [PermissionAuthorize("privacypolicy.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("privacypolicy.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ApiErrorResponse(PrivacyPolicyNotFoundCode, StatusCodes.Status404NotFound));
            }

            return Ok(item);
        }

        [HttpPost]
        [PermissionAuthorize("privacypolicy.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreatePrivacyPolicyRequestDto dto,
            [FromServices] IValidator<CreatePrivacyPolicyRequestDto> validator)
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
        [PermissionAuthorize("privacypolicy.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePrivacyPolicyRequestDto dto,
            [FromServices] IValidator<UpdatePrivacyPolicyRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ApiErrorResponse(PrivacyPolicyNotFoundCode, StatusCodes.Status404NotFound));
            }

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
            catch (Exception ex) when (ex.Message == "Privacy policy not found")
            {
                return NotFound(new ApiErrorResponse(PrivacyPolicyNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("privacypolicy.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ApiErrorResponse(PrivacyPolicyNotFoundCode, StatusCodes.Status404NotFound));
            }

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Privacy policy not found")
            {
                return NotFound(new ApiErrorResponse(PrivacyPolicyNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("privacypolicy.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeletePrivacyPolicyRequest request)
        {
            if (request.PrivacyPolicyIds == null || !request.PrivacyPolicyIds.Any())
            {
                return BadRequest(new ApiErrorResponse(PrivacyPolicyIdsRequiredCode, StatusCodes.Status400BadRequest));
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var privacyPolicyId in request.PrivacyPolicyIds)
            {
                try
                {
                    await _service.DeleteAsync(privacyPolicyId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(privacyPolicyId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}
