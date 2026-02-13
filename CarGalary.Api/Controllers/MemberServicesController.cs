

// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
// [Route("api/[controller]")]
// public class MemberServicesController : ControllerBase
// {
//     private readonly IMemberServiceService _service;

//     public MemberServicesController(IMemberServiceService service)
//     {
//         _service = service;
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//         => Ok(await _service.GetAllAsync());

//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var result = await _service.GetByIdAsync(id);
//         return result == null ? NotFound() : Ok(result);
//     }

//     [HttpPost]
//     [Authorize]
//     public async Task<IActionResult> Create(MemberService memberService)
//     {
//         var created = await _service.CreateAsync(memberService);
//         return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//     }

//     [HttpPut("{id}")]
//     [Authorize]
//     public async Task<IActionResult> Update(int id, MemberService memberService)
//     {
//         var updated = await _service.UpdateAsync(id, memberService);
//         return updated ? Ok() : NotFound();
//     }

//     [HttpDelete("{id}")]
//     [Authorize]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var deleted = await _service.DeleteAsync(id);
//         return deleted ? Ok() : NotFound();
//     }
// }

// }