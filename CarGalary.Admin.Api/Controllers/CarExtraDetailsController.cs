
using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.CarExtraDetails.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarExtraDetailsController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string CarExtraNotFoundCode = "1208";
        private const string CarIdInvalidCode = "1211";
        private const string ExtraDetailIdsRequiredCode = "1214";

        private readonly ICarExtraDetailsService _service;

        public CarExtraDetailsController(ICarExtraDetailsService service)
        {
            _service = service;
        }

        [HttpGet]
        [PermissionAuthorize("carextradetails.view")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("carextradetails.view")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
                }
                return Ok(item);
            }
            catch (Exception ex) when (ex.Message == "CarExtraDetails not found")
            {
                return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("by-car/{carId:int}")]
        [PermissionAuthorize("carextradetails.view")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            var items = await _service.GetByCarIdAsync(carId);
            return Ok(items);
        }

        [HttpPost]
        [PermissionAuthorize("carextradetails.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarExtraDetailsRequestDto dto,
            [FromServices] IValidator<CreateCarExtraDetailsRequestDto> validator)
        {
            try
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
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new ApiErrorResponse(CarIdInvalidCode, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("{id:int}")]
        [PermissionAuthorize("carextradetails.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarExtraDetailsRequestDto dto,
            [FromServices] IValidator<UpdateCarExtraDetailsRequestDto> validator)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
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
            catch (Exception ex) when (ex.Message == "CarExtraDetails not found")
            {
                return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new ApiErrorResponse(CarIdInvalidCode, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("carextradetails.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
                }

                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarExtraDetails not found")
            {
                return NotFound(new ApiErrorResponse(CarExtraNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("carextradetails.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteCarExtraDetailsRequest request)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return BadRequest(new ApiErrorResponse(ExtraDetailIdsRequiredCode, StatusCodes.Status400BadRequest));
            }

            var deletedCount = 0;
            var failedIds = new List<int>();

            foreach (var id in request.Ids)
            {
                try
                {
                    await _service.DeleteAsync(id);
                    deletedCount++;
                }
                catch
                {
                    failedIds.Add(id);
                }
            }

            return Ok(new { deletedCount, failedIds });
        }
    }
}
