using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Invoice.Command;
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
    public class InvoicesController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string InvoiceNotFoundCode = "1327";
        private const string InvoiceOperationFailedCode = "1328";

        private readonly IInvoiceService _invoiceService;
        private readonly ICarService _carService;
        private readonly IHubContext<CarHub> _hubContext;

        public InvoicesController(
            IInvoiceService invoiceService,
            ICarService carService,
            IHubContext<CarHub> hubContext)
        {
            _invoiceService = invoiceService;
            _carService = carService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _invoiceService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var item = await _invoiceService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(InvoiceNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateInvoiceRequestDto dto,
            [FromServices] IValidator<CreateInvoiceRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var result = await _invoiceService.CreateWithStockAsync(dto);

                foreach (var carId in result.Invoice.Details.Select(x => x.CarId).Distinct())
                {
                    var updatedCar = await _carService.GetByIdAsync(carId);
                    if (updatedCar != null)
                    {
                        await _hubContext.Clients.All.SendAsync("carUpdated", updatedCar);
                    }
                }

                foreach (var alert in result.LowStockAlerts)
                {
                    await _hubContext.Clients.All.SendAsync("carLowStock", alert);
                }

                return Ok(result.Invoice);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                return BadRequest(new ApiErrorResponse(
                    InvoiceOperationFailedCode,
                    StatusCodes.Status400BadRequest,
                    new List<string> { ex.Message }));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateInvoiceRequestDto dto,
            [FromServices] IValidator<UpdateInvoiceRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                var updated = await _invoiceService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(InvoiceNotFoundCode, StatusCodes.Status404NotFound));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                return BadRequest(new ApiErrorResponse(InvoiceOperationFailedCode, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _invoiceService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(InvoiceNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
