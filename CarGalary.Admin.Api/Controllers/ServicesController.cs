
using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _service;
        
        public ServicesController(IServicesService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [PermissionAuthorize("services.view")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        
        [HttpGet("{id:int}")]
        [PermissionAuthorize("services.view")]
        public async Task<IActionResult> GetById(int id)
        {
            var x = await _service.GetByIdAsync(id);
            return x == null ? NotFound() : Ok(x);
        }
        
        [HttpPost]
        [PermissionAuthorize("services.create")]
        public async Task<IActionResult> Create([FromForm] CreateServicesRequestDto dto, [FromServices] IValidator<CreateServicesRequestDto> v)
        {
            var r = v.Validate(dto);
            if (!r.IsValid) return BadRequest(r.Errors.Select(e => e.ErrorMessage).ToList());
            return Ok(await _service.CreateAsync(dto));
        }
        
        [HttpPut("{id:int}")]
        [PermissionAuthorize("services.edit")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateServicesRequestDto dto, [FromServices] IValidator<UpdateServicesRequestDto> v)
        {
            var ex = await _service.GetByIdAsync(id);
            if (ex == null) return NotFound();
            var r = v.Validate(dto);
            if (!r.IsValid) return BadRequest(r.Errors.Select(e => e.ErrorMessage).ToList());
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception e) when (e.Message == "Services not found")
            {
                return NotFound();
            }
        }
        
        [HttpDelete("{id:int}")]
        [PermissionAuthorize("services.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ex = await _service.GetByIdAsync(id);
            if (ex == null) return NotFound();
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e) when (e.Message == "Services not found")
            {
                return NotFound();
            }
        }
        
        [HttpPost("bulk-delete")]
        [PermissionAuthorize("services.delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteServicesRequestDto dto)
        {
            var result = await _service.BulkDeleteAsync(dto.ServiceIds);
            return Ok(result);
        }
    }
}
