using CarGalary.Application.Dtos.Services.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _service; public ServicesController(IServicesService service){_service=service;}
        [HttpGet] public async Task<IActionResult> GetAll()=>Ok(await _service.GetAllAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id){var x=await _service.GetByIdAsync(id); return x==null?NotFound():Ok(x);} 
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateServicesRequestDto dto,[FromServices]IValidator<CreateServicesRequestDto> v){var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); try{return Ok(await _service.CreateAsync(dto));} catch(Exception e) when (e.Message=="ServiceType not found"){return BadRequest(new[]{"ServiceTypeId is not valid"});}}
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,[FromBody]UpdateServicesRequestDto dto,[FromServices]IValidator<UpdateServicesRequestDto> v){var ex=await _service.GetByIdAsync(id); if(ex==null) return NotFound(); var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); try{await _service.UpdateAsync(id,dto); return Ok();} catch(Exception e) when (e.Message=="Services not found"){return NotFound();} catch(Exception e) when (e.Message=="ServiceType not found"){return BadRequest(new[]{"ServiceTypeId is not valid"});}}
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id){var ex=await _service.GetByIdAsync(id); if(ex==null) return NotFound(); try{await _service.DeleteAsync(id); return Ok();} catch(Exception e) when (e.Message=="Services not found"){return NotFound();}}
    }
}
