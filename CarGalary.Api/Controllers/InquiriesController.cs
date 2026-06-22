using System.Security.Claims;
using CarGalary.Application.Dtos.Frontend;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/inquiries")]
    public class InquiriesController : ApiControllerBase
    {
        private const string UserNotFoundCode = "1227";

        private readonly IFrontendApiService _frontendApiService;

        public InquiriesController(IFrontendApiService frontendApiService)
        {
            _frontendApiService = frontendApiService;
        }

        [HttpGet]
        public async Task<ActionResult<FrontendInquiriesResponseDto>> GetInquiries()
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            var inquiries = await _frontendApiService.GetInquiriesAsync(userId);
            return Ok(new FrontendInquiriesResponseDto { Inquiries = inquiries });
        }

        [HttpPost]
        public async Task<ActionResult<FrontendInquiryResponseDto>> CreateInquiry(
            [FromBody] FrontendCreateInquiryRequestDto request)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var inquiry = await _frontendApiService.CreateInquiryAsync(userId, email, request);
            return Ok(new FrontendInquiryResponseDto { Inquiry = inquiry });
        }

        private bool TryGetCurrentUserId(out Guid userId)
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out userId);
        }
    }
}
