using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.ErrorCatalog;
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
        private readonly IQuotationService _quotationService;
        private readonly IHubContext<QuotationHub> _hubContext;
        private readonly IErrorCatalogService _errorCatalogService;

        public QuotationsController(
            IQuotationService quotationService,
            IHubContext<QuotationHub> hubContext,
            IErrorCatalogService errorCatalogService)
        {
            _quotationService = quotationService;
            _hubContext = hubContext;
            _errorCatalogService = errorCatalogService;
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
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
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
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
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
                return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var created = await _quotationService.CreateAsync(dto);
                await _hubContext.Clients.All.SendAsync("quotationCreated", created);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(BuildErrorByCode(ex.Message, StatusCodes.Status400BadRequest));
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
                return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var updated = await _quotationService.UpdateStatusAsync(id, dto);
                await _hubContext.Clients.All.SendAsync("quotationStatusUpdated", updated);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(BuildErrorByCode(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        private ApiErrorResponse BuildErrorByCode(string errorCodeOrMessage, int statusCode)
        {
            var code = errorCodeOrMessage?.Trim() ?? string.Empty;
            var entry = _errorCatalogService.GetByCode(code);
            if (entry == null)
            {
                return new ApiErrorResponse(errorCodeOrMessage, statusCode);
            }

            var message = IsArabicRequest() ? entry.MessageAr : entry.MessageEn;
            return new ApiErrorResponse(
                message,
                statusCode,
                errorCode: entry.ErrorCode,
                messageAr: entry.MessageAr,
                messageEn: entry.MessageEn);
        }

        private bool IsArabicRequest()
        {
            var acceptLanguage = Request.Headers.AcceptLanguage.ToString();
            return !string.IsNullOrWhiteSpace(acceptLanguage) &&
                   acceptLanguage.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }
    }
}
