using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/Quotations")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _requestService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _requestService.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory([FromRoute] int id)
        {
            try
            {
                var request = await _requestService.GetByIdAsync(id);
                if (request == null || !request.IsAvailable)
                {

                    return NotFound(new ApiErrorResponse($"Request not found for id #{id}", StatusCodes.Status404NotFound));
                }
                var items = await _requestService.GetHistoryAsync(id);
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
            [FromBody] CreateRequestDto dto,
            [FromServices] IValidator<CreateRequestDto> validator)
        {
            try
            {
                var created = await _requestService.CreateAsync(dto);
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
            [FromBody] UpdateRequestStatusDto dto,
            [FromServices] IValidator<UpdateRequestStatusDto> validator)
        {
            try
            {
                var updated = await _requestService.UpdateStatusAsync(id, dto);
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
