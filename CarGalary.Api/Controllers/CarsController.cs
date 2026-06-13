

using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Frontend;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ApiControllerBase
    {
        private const string CarNotFoundCode = "1209";

        private readonly ICarService _carService;
        private readonly IFrontendApiService _frontendApiService;

        public CarsController(ICarService carService, IFrontendApiService frontendApiService)
        {
            _carService = carService;
            _frontendApiService = frontendApiService;
        }

        [HttpGet]
        public async Task<ActionResult<FrontendCarsResponseDto>> GetCars([FromQuery] FrontendCarQueryDto query)
        {
            var cars = await _frontendApiService.GetCarsAsync(query);
            return Ok(cars);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<CarApiResponseDto>>> GetLatestCars()
        {
            var cars = await _carService.GetAllForApiAsync();
            var latestCars = cars
                .OrderByDescending(c => c.CreatedAt)
                .Take(10)
                .ToList();

            return Ok(latestCars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FrontendCarResponseDto>> GetCar(int id)
        {
            var car = await _frontendApiService.GetCarAsync(id);
            if (car == null)
            {
                return NotFoundErrorResponse(CarNotFoundCode);
            }

            return Ok(new FrontendCarResponseDto { Car = car });
        }

        [HttpGet("by-model/{modelId:int}")]
        public async Task<ActionResult<IEnumerable<CarApiResponseDto>>> GetCarsByModel(int modelId)
        {
            var cars = await _carService.FilterForApiAsync(modelId: modelId);
            return Ok(cars);
        }

        
    }
}
