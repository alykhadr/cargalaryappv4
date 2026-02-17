using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteController : ControllerBase
    {
        private readonly IFavoritesService _service; public UserFavoriteController(IFavoritesService service){_service=service;}
        [HttpGet] public async Task<IActionResult> GetAll()=>Ok(await _service.GetAllAsync());
        [HttpGet("{userId:guid}/{carId:int}")] public async Task<IActionResult> GetById(Guid userId,int carId){var x=await _service.GetByIdAsync(userId,carId); return x==null?NotFound():Ok(x);} 
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateUserFavoriteAdminRequestDto dto,[FromServices]IValidator<CreateUserFavoriteAdminRequestDto> v){var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); return Ok(await _service.CreateAsync(dto));}
        [HttpPut("{userId:guid}/{carId:int}")] public async Task<IActionResult> Update(Guid userId,int carId,[FromBody]UpdateUserFavoriteAdminRequestDto dto,[FromServices]IValidator<UpdateUserFavoriteAdminRequestDto> v){var ex=await _service.GetByIdAsync(userId,carId); if(ex==null) return NotFound(); var r=v.Validate(dto); if(!r.IsValid) return BadRequest(r.Errors.Select(e=>e.ErrorMessage).ToList()); try{await _service.UpdateAsync(userId,carId,dto); return Ok();} catch(Exception e) when (e.Message=="UserFavorite not found"){return NotFound();}}
        [HttpDelete("{userId:guid}/{carId:int}")] public async Task<IActionResult> Delete(Guid userId,int carId){var ex=await _service.GetByIdAsync(userId,carId); if(ex==null) return NotFound(); try{await _service.DeleteAsync(userId,carId); return Ok();} catch(Exception e) when (e.Message=="UserFavorite not found"){return NotFound();}}
    }
}
