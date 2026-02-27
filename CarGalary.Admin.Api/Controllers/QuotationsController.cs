using CarGalary.Application.Dtos.Quotation.Command;
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

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateQuotationRequestDto dto,
            [FromServices] IValidator<CreateQuotationRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var created = await _quotationService.CreateAsync(dto);
            await _hubContext.Clients.All.SendAsync("quotationCreated", created);
            return Ok(created);
        }
    }
}
