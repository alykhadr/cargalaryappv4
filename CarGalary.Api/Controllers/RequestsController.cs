using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/Requests")]
    public class RequestsController : ApiControllerBase
    {
        private const string RequestOperationFailedCode = "1322";

        private readonly IRequestService _requestService;
        private readonly IBrandService _brandService;

        public RequestsController(IRequestService requestService, IBrandService brandService)
        {
            _requestService = requestService;
            _brandService = brandService;
        }


        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] int take = 10)
        {
            var notifications = await _requestService.GetNotificationsAsync(take);
            return Ok(notifications);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRequestDto dto,
            [FromServices] IValidator<CreateRequestDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequestErrorResponse(
                    errors: validation.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var created = await _requestService.CreateAsync(dto);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequestErrorResponse(ResolveBadRequestCode(ex, RequestOperationFailedCode));
            }
        }

        
    }
}
