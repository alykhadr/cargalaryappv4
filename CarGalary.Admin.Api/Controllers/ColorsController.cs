using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.CarColor.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColorsController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string CarColorNotFoundCode = "1302";
        private const string ColorIdsRequiredCode = "1212";

        private readonly ICarColorService _carColorService;

        public ColorsController(ICarColorService carColorService)
        {
            _carColorService = carColorService;
        }

        [HttpGet]
        [PermissionAuthorize("colors.view")]
        public async Task<IActionResult> GetAll()
        {
            var colors = await _carColorService.GetAllAsync();
            return Ok(colors);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("colors.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _carColorService.GetByIdAsync(id);
            if (color == null)
            {
                return NotFound(new ApiErrorResponse(CarColorNotFoundCode, StatusCodes.Status404NotFound));
            }

            return Ok(color);
        }

        [HttpPost]
        [PermissionAuthorize("colors.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarColorRequestDto createCarColorRequestDto,
            [FromServices] IValidator<CreateCarColorRequestDto> validator)
        {
            var validationResult = validator.Validate(createCarColorRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            var created = await _carColorService.CreateAsync(createCarColorRequestDto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("colors.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarColorRequestDto updateCarColorRequestDto,
            [FromServices] IValidator<UpdateCarColorRequestDto> validator)
        {
            var existingColor = await _carColorService.GetByIdAsync(id);
            if (existingColor == null)
            {
                return NotFound(new ApiErrorResponse(CarColorNotFoundCode, StatusCodes.Status404NotFound));
            }

            var validationResult = validator.Validate(updateCarColorRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
            }

            try
            {
                await _carColorService.UpdateAsync(id, updateCarColorRequestDto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarColor not found")
            {
                return NotFound(new ApiErrorResponse(CarColorNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("colors.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _carColorService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarColor not found")
            {
                return NotFound(new ApiErrorResponse(CarColorNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("colors.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteColorsRequest request)
        {
            if (request.ColorIds == null || !request.ColorIds.Any())
            {
                return BadRequest(new ApiErrorResponse(ColorIdsRequiredCode, StatusCodes.Status400BadRequest));
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var colorId in request.ColorIds)
            {
                try
                {
                    await _carColorService.DeleteAsync(colorId);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(colorId);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}
