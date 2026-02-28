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
        private readonly IQuotationService _quotationService;
        private readonly IHubContext<QuotationHub> _hubContext;

        public QuotationsController(IQuotationService quotationService, IHubContext<QuotationHub> hubContext)
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
            var item = await _quotationService.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory([FromRoute] int id)
        {
            var items = await _quotationService.GetHistoryAsync(id);
            return Ok(items);
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

            var created = await _quotationService.CreateAsync(dto);
            await _hubContext.Clients.All.SendAsync("quotationCreated", created);
            return Ok(created);
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

            var updated = await _quotationService.UpdateStatusAsync(id, dto);
            await _hubContext.Clients.All.SendAsync("quotationStatusUpdated", updated);
            return Ok(updated);
        }
    }
}
