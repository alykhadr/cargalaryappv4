using CarGalary.Application.Dtos.Quotation.Command;
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
    [ApiController]
    [Authorize]
        public class QuotationsController : ControllerBase
        {
            private const string ValidationFailedCode = "1101";
            private const string QuotationNotFoundCode = "1321";
            private const string QuotationOperationFailedCode = "1322";

            private readonly IQuotationService _quotationService;
            private readonly IHubContext<QuotationHub> _hubContext;

            public QuotationsController(
                IQuotationService quotationService,
                IHubContext<QuotationHub> hubContext)
            {
                _quotationService = quotationService;
                _hubContext = hubContext;
            }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _quotationService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var item = await _quotationService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(QuotationNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory([FromRoute] int id)
        {
            try
            {
                var items = await _quotationService.GetHistoryAsync(id);
                return Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(QuotationNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateQuotationRequestDto dto,
            [FromServices] IValidator<CreateQuotationRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var created = await _quotationService.CreateAsync(dto);
                await _hubContext.Clients.All.SendAsync("quotationCreated", created);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                var code = !string.IsNullOrWhiteSpace(ex.Message) && ex.Message.All(char.IsDigit) ? ex.Message : QuotationOperationFailedCode;
                return BadRequest(new ApiErrorResponse(code, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateQuotationStatusRequestDto dto,
            [FromServices] IValidator<UpdateQuotationStatusRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var updated = await _quotationService.UpdateStatusAsync(id, dto);
                await _hubContext.Clients.All.SendAsync("quotationStatusUpdated", updated);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                var code = !string.IsNullOrWhiteSpace(ex.Message) && ex.Message.All(char.IsDigit) ? ex.Message : QuotationOperationFailedCode;
                return BadRequest(new ApiErrorResponse(code, StatusCodes.Status400BadRequest));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(QuotationNotFoundCode, StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(QuotationOperationFailedCode, StatusCodes.Status400BadRequest));
            }
        }
    }
}
