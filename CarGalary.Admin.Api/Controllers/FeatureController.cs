using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string CarFeatureNotFoundCode = "1303";

        private readonly ICarFeatureService _service;

        public FeatureController(ICarFeatureService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var features = await _service.GetAllAsync();
            return Ok(features);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feature = await _service.GetByIdAsync(id);
            if (feature == null) return NotFound(new ApiErrorResponse(CarFeatureNotFoundCode, StatusCodes.Status404NotFound));
            return Ok(feature);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarFeatureRequestDto dto,
            [FromServices] IValidator<CreateCarFeatureRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarFeatureRequestDto dto,
            [FromServices] IValidator<UpdateCarFeatureRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(CarFeatureNotFoundCode, StatusCodes.Status404NotFound));

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarFeature not found")
            {
                return NotFound(new ApiErrorResponse(CarFeatureNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound(new ApiErrorResponse(CarFeatureNotFoundCode, StatusCodes.Status404NotFound));

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarFeature not found")
            {
                return NotFound(new ApiErrorResponse(CarFeatureNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
