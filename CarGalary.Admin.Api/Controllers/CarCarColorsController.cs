using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarCarColorsController : ControllerBase
    {
        private readonly ICarCarColorService _carCarColorService;

        public CarCarColorsController(ICarCarColorService carCarColorService)
        {
            _carCarColorService = carCarColorService;
        }

        [HttpGet("by-car/{carId:int}")]
        [PermissionAuthorize("cars.view")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            try
            {
                var items = await _carCarColorService.GetByCarIdAsync(carId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
