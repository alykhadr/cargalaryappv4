using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.CarType.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypesController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string CarTypeNotFoundCode = "1210";

        private readonly ICarTypeService _service;

        public TypesController(ICarTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        [PermissionAuthorize("types.view")]
        public async Task<IActionResult> GetAll()
        {
            var types = await _service.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("types.view")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var type = await _service.GetByIdAsync(id);
                if (type == null)
                {
                    return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
                }
                return Ok(type);
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost]
        [PermissionAuthorize("types.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarTypeRequestDto dto,
            [FromServices] IValidator<CreateCarTypeRequestDto> validator)
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
        [PermissionAuthorize("types.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarTypeRequestDto dto,
            [FromServices] IValidator<UpdateCarTypeRequestDto> validator)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
                }

                var validationResult = validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse(ValidationFailedCode, StatusCodes.Status400BadRequest, errors));
                }

                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("types.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
                }

                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return NotFound(new ApiErrorResponse(CarTypeNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
