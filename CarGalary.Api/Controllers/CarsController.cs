

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
        public async Task<ActionResult<IEnumerable<CarResponseDto>>> GetCars()
        {
            var cars = await _carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarResponseDto>> GetCar(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpGet("by-model/{modelId:int}")]
        public async Task<ActionResult<IEnumerable<CarResponseDto>>> GetCarsByModel(int modelId)
        {
            var cars = await _carService.FilterAsync(modelId: modelId);
            return Ok(cars);
        }

         [Authorize(Roles="Admin,Manager")]
        [HttpPost]
        public async Task<ActionResult<CarResponseDto>> CreateCar(CreateCarRequestDto car)
        {
            var createdCar = await _carService.CreateAsync(car);
            return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
        }
         [Authorize(Roles="Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, UpdateCarRequestDto car)
        {
            await _carService.UpdateAsync(id, car);
            return NoContent();
        }
         [Authorize(Roles="Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CarResponseDto>>> FilterCars([FromQuery] int? modelId, [FromQuery] int? typeId, [FromQuery] bool? isAvailable)
        {
            var cars = await _carService.FilterAsync(modelId, typeId, isAvailable);
            return Ok(cars);
        }
    }
}