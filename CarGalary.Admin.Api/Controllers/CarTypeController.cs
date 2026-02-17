using CarGalary.Application.Dtos.CarType.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        private readonly ICarTypeService _service;

        public CarTypeController(ICarTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _service.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _service.GetByIdAsync(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCarTypeRequestDto dto,
            [FromServices] IValidator<CreateCarTypeRequestDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCarTypeRequestDto dto,
            [FromServices] IValidator<UpdateCarTypeRequestDto> validator)
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
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return NotFound();
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
            catch (Exception ex) when (ex.Message == "CarType not found")
            {
                return NotFound();
            }
        }
    }
}
