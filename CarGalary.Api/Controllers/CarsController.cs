

using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Dtos.Car.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarApiResponseDto>>> GetCars()
        {
            var cars = await _carService.GetAllForApiAsync();
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
        public async Task<ActionResult<CarApiResponseDto>> GetCar(int id)
        {
            var car = await _carService.GetByIdForApiAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpGet("by-model/{modelId:int}")]
        public async Task<ActionResult<IEnumerable<CarApiResponseDto>>> GetCarsByModel(int modelId)
        {
            var cars = await _carService.FilterForApiAsync(modelId: modelId);
            return Ok(cars);
        }

        
    }
}
