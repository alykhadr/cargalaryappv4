using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Invoice.Command;
using CarGalary.Application.Interfaces;
using CarGalary.Application.Utilities;
using CarGalary.Admin.Api.Hubs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QRCoder;

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

        [HttpGet("{id:int}/zatca-qr.png")]
        [AllowAnonymous]
        public async Task<IActionResult> GetZatcaQrPng([FromRoute] int id)
        {
            try
            {
                var invoice = await _invoiceService.GetByIdAsync(id);
                if (string.IsNullOrWhiteSpace(invoice.ZatcaQrCode))
                {
                    return NotFound(new ApiErrorResponse(InvoiceOperationFailedCode, StatusCodes.Status404NotFound, new List<string>
                    {
                        "ZATCA QR code is not available for this invoice."
                    }));
                }

                using var qrGenerator = new QRCodeGenerator();
                using var qrData = qrGenerator.CreateQrCode(invoice.ZatcaQrCode, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrData);
                var pngBytes = qrCode.GetGraphic(20);

                Response.Headers.CacheControl = "public,max-age=86400";
                return File(pngBytes, "image/png");
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiErrorResponse(InvoiceNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("zatca-qr-preview.png")]
        [AllowAnonymous]
        public IActionResult GetZatcaQrPreviewPng(
            [FromQuery] string sellerName,
            [FromQuery] string vatRegistrationNumber,
            [FromQuery] DateTime issueDate,
            [FromQuery] decimal invoiceTotalWithVat,
            [FromQuery] decimal vatTotal)
        {
            if (string.IsNullOrWhiteSpace(sellerName) || string.IsNullOrWhiteSpace(vatRegistrationNumber))
            {
                return BadRequest(new ApiErrorResponse(
                    InvoiceOperationFailedCode,
                    StatusCodes.Status400BadRequest,
                    new List<string> { "Seller name and VAT registration number are required for QR preview." }));
            }

            try
            {
                var payload = ZatcaQrCodeBuilder.BuildPhaseOnePayload(
                    sellerName.Trim(),
                    vatRegistrationNumber.Trim(),
                    issueDate,
                    invoiceTotalWithVat,
                    vatTotal);

                using var qrGenerator = new QRCodeGenerator();
                using var qrData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrData);
                var pngBytes = qrCode.GetGraphic(20);

                Response.Headers.CacheControl = "no-store";
                return File(pngBytes, "image/png");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ApiErrorResponse(
                    InvoiceOperationFailedCode,
                    StatusCodes.Status400BadRequest,
                    new List<string> { ex.Message }));
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
                return BadRequest(new ApiErrorResponse(
                    InvoiceOperationFailedCode,
                    StatusCodes.Status400BadRequest,
                    new List<string> { ex.Message }));
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
