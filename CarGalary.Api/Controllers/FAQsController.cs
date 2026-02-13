
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
// [Route("api/[controller]")]
// public class FAQsController : ControllerBase
// {
//     private readonly IFAQService _service;

//     public FAQsController(IFAQService service)
//     {
//         _service = service;
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//         => Ok(await _service.GetAllAsync());

//     [HttpGet("available")]
//     public async Task<IActionResult> GetAvailable()
//         => Ok(await _service.GetAvailableAsync());

//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var result = await _service.GetByIdAsync(id);
//         return result == null ? NotFound() : Ok(result);
//     }

//     [HttpPost]
//     [Authorize]
//     public async Task<IActionResult> Create(FAQ faq)
//     {
//         var created = await _service.CreateAsync(faq);
//         return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//     }

//     [HttpPut("{id}")]
//      [Authorize]
//     public async Task<IActionResult> Update(int id, FAQ faq)
//     {
//         var updated = await _service.UpdateAsync(id, faq);
//         return updated ? Ok() : NotFound();
//     }

//     [HttpDelete("{id}")]
//      [Authorize]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var deleted = await _service.DeleteAsync(id);
//         return deleted ? Ok() : NotFound();
//     }
// }

// }