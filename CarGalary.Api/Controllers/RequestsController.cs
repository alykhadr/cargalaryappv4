using CarGalary.Application.Dtos.Request.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/Requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IBrandService _brandService;

        public RequestsController(IRequestService requestService, IBrandService brandService)
        {
            _requestService = requestService;
            _brandService = brandService;
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

        
    }
}
