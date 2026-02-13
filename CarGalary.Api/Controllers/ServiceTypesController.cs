

// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]

//     public class ServiceTypesController : ControllerBase
//     {
//         private readonly IServiceTypeService _service;

//         public ServiceTypesController(IServiceTypeService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             return Ok(await _service.GetAllAsync());
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(int id)
//         {
//             var result = await _service.GetByIdAsync(id);
//             if (result == null) return NotFound();
//             return Ok(result);
//         }

//         [HttpPost]
//         [Authorize]
//         public async Task<IActionResult> Create(ServiceType serviceType)
//         {
//             var created = await _service.CreateAsync(serviceType);
//             return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         [Authorize]
//         public async Task<IActionResult> Update(int id, ServiceType serviceType)
//         {
//             var updated = await _service.UpdateAsync(id, serviceType);
//             if (!updated) return NotFound();
//             return Ok();
//         }

//         [HttpDelete("{id}")]
//         [Authorize]
//         public async Task<IActionResult> Delete(int id)
//         {
//             var deleted = await _service.DeleteAsync(id);
//             if (!deleted) return NotFound();
//             return Ok();
//         }
//     }
// }