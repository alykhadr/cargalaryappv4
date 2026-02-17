using CarGalary.Application.Dtos.MemberService.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberServiceController : ControllerBase
    {
        private readonly IMemberServiceService _service;
        public MemberServiceController(IMemberServiceService service){_service=service;}
        [HttpGet] public async Task<IActionResult> GetAll()=>Ok(await _service.GetAllAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id){var x=await _service.GetByIdAsync(id);return x==null?NotFound():Ok(x);} 
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateMemberServiceRequestDto dto,[FromServices] IValidator<CreateMemberServiceRequestDto> v){var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); return Ok(await _service.CreateAsync(dto));}
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,[FromBody]UpdateMemberServiceRequestDto dto,[FromServices] IValidator<UpdateMemberServiceRequestDto> v){var ex=await _service.GetByIdAsync(id); if(ex==null) return NotFound(); var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); try{await _service.UpdateAsync(id,dto); return Ok();} catch(Exception e) when (e.Message=="MemberService not found"){return NotFound();}}
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id){var ex=await _service.GetByIdAsync(id); if(ex==null) return NotFound(); try{await _service.DeleteAsync(id); return Ok();} catch(Exception e) when (e.Message=="MemberService not found"){return NotFound();}}
    }
}
