using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using CarGalary.Admin.Api.Hubs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/Quotations")]
    [ApiController]
    [Authorize]
        public class RequestsController : ControllerBase
        {
            private const string ValidationFailedCode = "1101";
            private const string RequestNotFoundCode = "1321";
            private const string RequestOperationFailedCode = "1322";

            private readonly IRequestService _requestService;
            private readonly IHubContext<RequestHub> _hubContext;

            public RequestsController(
                IRequestService requestService,
                IHubContext<RequestHub> hubContext)
            {
                _requestService = requestService;
                _hubContext = hubContext;
            }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _requestService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] int take = 10)
        {
            var items = await _requestService.GetNotificationsAsync(take);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var item = await _requestService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(RequestNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory([FromRoute] int id)
        {
            try
            {
                var items = await _requestService.GetHistoryAsync(id);
                return Ok(items);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(RequestNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRequestDto dto,
            [FromServices] IValidator<CreateRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var created = await _requestService.CreateAsync(dto);
                await _hubContext.Clients.All.SendAsync("requestCreated", created);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                var code = !string.IsNullOrWhiteSpace(ex.Message) && ex.Message.All(char.IsDigit) ? ex.Message : RequestOperationFailedCode;
                return BadRequest(new ApiErrorResponse(code, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateRequestStatusDto dto,
            [FromServices] IValidator<UpdateRequestStatusDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var updated = await _requestService.UpdateStatusAsync(id, dto);
                await _hubContext.Clients.All.SendAsync("requestStatusUpdated", updated);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                var code = !string.IsNullOrWhiteSpace(ex.Message) && ex.Message.All(char.IsDigit) ? ex.Message : RequestOperationFailedCode;
                return BadRequest(new ApiErrorResponse(code, StatusCodes.Status400BadRequest));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(RequestNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
