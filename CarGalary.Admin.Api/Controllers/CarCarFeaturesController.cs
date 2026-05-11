using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.CarFeature.Command;
using CarGalary.Application.Dtos.CarFeature.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarCarFeaturesController : ControllerBase
    {
        private const string FeatureIdRequiredCode = "1308";
        private const string CarIdInvalidCode = "1309";
        private const string FeatureIdInvalidCode = "1310";
        private const string FeatureAlreadyAssignedCode = "1311";
        private const string CarFeatureAssignmentNotFoundCode = "1312";

        private readonly ICarFeatureService _carFeatureService;

        public CarCarFeaturesController(ICarFeatureService carFeatureService)
        {
            _carFeatureService = carFeatureService;
        }

        [HttpGet("by-car/{carId:int}")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            var items = await _carFeatureService.GetAssignmentsByCarIdAsync(carId);
            return Ok(items);
        }

        [HttpPost("{carId:int}")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> Create(int carId, [FromBody] CreateCarFeatureAssignmentRequestDto dto)
        {
            try
            {
                var created = await _carFeatureService.CreateAssignmentAsync(carId, dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "FeatureId is required")
            {
                return BadRequest(new ApiErrorResponse(FeatureIdRequiredCode, StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "CarId is not valid")
            {
                return BadRequest(new ApiErrorResponse(CarIdInvalidCode, StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "FeatureId is not valid")
            {
                return BadRequest(new ApiErrorResponse(FeatureIdInvalidCode, StatusCodes.Status400BadRequest));
            }
            catch (Exception ex) when (ex.Message == "Feature already assigned to this car")
            {
                return Conflict(new ApiErrorResponse(FeatureAlreadyAssignedCode, StatusCodes.Status409Conflict));
            }
        }

        [HttpPut("{carId:int}/{featureId:int}")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> Update(int carId, int featureId, [FromBody] UpdateCarFeatureAssignmentRequestDto dto)
        {
            try
            {
                await _carFeatureService.UpdateAssignmentAsync(carId, featureId, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car feature assignment not found")
            {
                return NotFound(new ApiErrorResponse(CarFeatureAssignmentNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{carId:int}/{featureId:int}")]
        [PermissionAuthorize("cars.edit")]
        public async Task<IActionResult> Delete(int carId, int featureId)
        {
            try
            {
                await _carFeatureService.DeleteAssignmentAsync(carId, featureId);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car feature assignment not found")
            {
                return NotFound(new ApiErrorResponse(CarFeatureAssignmentNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
