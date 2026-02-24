
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
        private readonly ICarExtraDetailsService _service;

        public CarExtraDetailsController(ICarExtraDetailsService service)
        {
            _service = service;
        }

        [HttpGet]
        [PermissionAuthorize("carextradetails.view")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
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
                    return NotFound(new ApiErrorResponse("Car extra detail not found", StatusCodes.Status404NotFound));
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("by-car/{carId:int}")]
        [PermissionAuthorize("carextradetails.view")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            try
            {
                var items = await _service.GetByCarIdAsync(carId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
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
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new ApiErrorResponse("CarId is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
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
                    return NotFound(new ApiErrorResponse("Car extra detail not found", StatusCodes.Status404NotFound));
                }

                var validationResult = validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarExtraDetails not found")
            {
                return NotFound(new ApiErrorResponse("Car extra detail not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new ApiErrorResponse("CarId is not valid", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
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
                    return NotFound(new ApiErrorResponse("Car extra detail not found", StatusCodes.Status404NotFound));
                }

                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "CarExtraDetails not found")
            {
                return NotFound(new ApiErrorResponse("Car extra detail not found", StatusCodes.Status404NotFound));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("bulk-delete")]
        [PermissionAuthorize("carextradetails.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteCarExtraDetailsRequest request)
        {
            try
            {
                if (request.Ids == null || !request.Ids.Any())
                {
                    return BadRequest(new ApiErrorResponse("Extra detail IDs are required", StatusCodes.Status400BadRequest));
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}

