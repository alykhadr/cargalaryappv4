using CarGalary.Application.Dtos.FAQ.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _service;
        public FAQController(IFAQService service) { _service = service; }

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id){var x=await _service.GetByIdAsync(id); return x==null?NotFound():Ok(x);} 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFAQRequestDto dto, [FromServices] IValidator<CreateFAQRequestDto> validator)
        {
            var v = validator.Validate(dto); if (!v.IsValid) return BadRequest(v.Errors.Select(e => e.ErrorMessage).ToList());
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFAQRequestDto dto, [FromServices] IValidator<UpdateFAQRequestDto> validator)
        {
            var ex = await _service.GetByIdAsync(id); if (ex == null) return NotFound();
            var v = validator.Validate(dto); if (!v.IsValid) return BadRequest(v.Errors.Select(e => e.ErrorMessage).ToList());
            try { await _service.UpdateAsync(id, dto); return Ok(); } catch (Exception e) when (e.Message == "FAQ not found") { return NotFound(); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ex = await _service.GetByIdAsync(id); if (ex == null) return NotFound();
            try { await _service.DeleteAsync(id); return Ok(); } catch (Exception e) when (e.Message == "FAQ not found") { return NotFound(); }
        }
    }
}
