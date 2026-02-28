using CarGalary.Application.Dtos.Quotation.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationsController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationsController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
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
            try
            {
                var quotation = await _quotationService.GetByIdAsync(id);
                if (quotation == null || !quotation.IsAvailable)
                {

                    return NotFound(new ApiErrorResponse($"Quotation not found for id #{id}", StatusCodes.Status404NotFound));
                }
                var items = await _quotationService.GetHistoryAsync(id);
                return Ok(items);
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

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateQuotationRequestDto dto,
            [FromServices] IValidator<CreateQuotationRequestDto> validator)
        {
            try
            {
                var created = await _quotationService.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }


        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateQuotationStatusRequestDto dto,
            [FromServices] IValidator<UpdateQuotationStatusRequestDto> validator)
        {
            try
            {
                var updated = await _quotationService.UpdateStatusAsync(id, dto);
                return Ok(updated);
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
    }
}
