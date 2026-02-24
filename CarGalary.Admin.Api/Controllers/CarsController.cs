using CarGalary.Application.Dtos.Car.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _service;

        public CarsController(ICarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _service.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _service.GetByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] int? modelId, [FromQuery] int? typeId, [FromQuery] bool? isAvailable)
        {
            var cars = await _service.FilterAsync(modelId, typeId, isAvailable);
            return Ok(cars);
        }

        [HttpGet("by-model/{modelId:int}")]
        public async Task<IActionResult> GetCarsByModel(int modelId)
        {
            var cars = await _service.FilterAsync(modelId: modelId);
            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarRequestDto dto,
            [FromServices] IValidator<CreateCarRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return BadRequest(new[] { "ModelId is not valid" });
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return BadRequest(new[] { "TypeId is not valid" });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarRequestDto dto,
            [FromServices] IValidator<UpdateCarRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound();
            }
            catch (Exception ex) when (ex.Message == "CarModel not found")
            {
                return BadRequest(new[] { "ModelId is not valid" });
            }
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return BadRequest(new[] { "TypeId is not valid" });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return NotFound();
            }
        }
    }
}
