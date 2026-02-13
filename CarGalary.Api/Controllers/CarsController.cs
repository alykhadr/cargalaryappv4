

// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CarsController : ControllerBase
//     {
//         private readonly ICarService _carService;

//         public CarsController(ICarService carService)
//         {
//             _carService = carService;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Car>>> GetCars()
//         {
//             var cars = await _carService.GetAllAsync();
//             return Ok(cars);
//         }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<Car>> GetCar(int id)
//         {
//             var car = await _carService.GetByIdAsync(id);
//             if (car == null) return NotFound();
//             return Ok(car);
//         }
//          [Authorize(Roles="Admin,Manager")]
//         [HttpPost]
//         public async Task<ActionResult<Car>> CreateCar(Car car)
//         {
//             var createdCar = await _carService.CreateAsync(car);
//             return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
//         }
//          [Authorize(Roles="Admin,Manager")]
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateCar(int id, Car car)
//         {
//             var result = await _carService.UpdateAsync(id, car);
//             if (!result) return BadRequest();
//             return NoContent();
//         }
//          [Authorize(Roles="Admin,Manager")]
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteCar(int id)
//         {
//             var result = await _carService.DeleteAsync(id);
//             if (!result) return NotFound();
//             return NoContent();
//         }

//         [HttpGet("filter")]
//         public async Task<ActionResult<IEnumerable<Car>>> FilterCars([FromQuery] int? brandId, [FromQuery] int? typeId, [FromQuery] bool? isAvailable)
//         {
//             var cars = await _carService.FilterAsync(brandId, typeId, isAvailable);
//             return Ok(cars);
//         }
//     }
// }