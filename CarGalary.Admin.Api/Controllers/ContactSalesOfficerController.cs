using CarGalary.Application.Dtos.ContactSalesOfficer.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactSalesOfficerController : ControllerBase
    {
        private readonly IContactSalesOfficerService _service;

        public ContactSalesOfficerController(IContactSalesOfficerService service)
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

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateContactSalesOfficerRequestDto dto,
            [FromServices] IValidator<CreateContactSalesOfficerRequestDto> validator)
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
            [FromBody] UpdateContactSalesOfficerRequestDto dto,
            [FromServices] IValidator<UpdateContactSalesOfficerRequestDto> validator)
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
            catch (Exception ex) when (ex.Message == "ContactSalesOfficer not found")
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
            catch (Exception ex) when (ex.Message == "ContactSalesOfficer not found")
            {
                return NotFound();
            }
        }
    }
}
