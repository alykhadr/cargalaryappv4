using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioAndCommunicationSystemController : ControllerBase
    {
        private readonly IAudioAndCommunicationSystemService _service;

        public AudioAndCommunicationSystemController(IAudioAndCommunicationSystemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("by-car/{carId:int}")]
        public async Task<IActionResult> GetByCarId(int carId)
        {
            var items = await _service.GetByCarIdAsync(carId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAudioAndCommunicationSystemRequestDto dto,
            [FromServices] IValidator<CreateAudioAndCommunicationSystemRequestDto> validator)
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
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new[] { "CarId is not valid" });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAudioAndCommunicationSystemRequestDto dto,
            [FromServices] IValidator<UpdateAudioAndCommunicationSystemRequestDto> validator)
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
            catch (Exception ex) when (ex.Message == "AudioAndCommunicationSystem not found")
            {
                return NotFound();
            }
            catch (Exception ex) when (ex.Message == "Car not found")
            {
                return BadRequest(new[] { "CarId is not valid" });
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
            catch (Exception ex) when (ex.Message == "AudioAndCommunicationSystem not found")
            {
                return NotFound();
            }
        }
    }
}

