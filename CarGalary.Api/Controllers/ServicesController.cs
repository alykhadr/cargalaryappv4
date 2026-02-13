
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
    
//     public class ServicesController : ControllerBase
//     {
//         private readonly IServicesService _service;

//         public ServicesController(IServicesService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//             => Ok(await _service.GetAllAsync());

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(int id)
//         {
//             var result = await _service.GetByIdAsync(id);
//             return result == null ? NotFound() : Ok(result);
//         }

//         [HttpGet("by-service-type/{serviceTypeId}")]
//         public async Task<IActionResult> GetByServiceType(int serviceTypeId)
//             => Ok(await _service.GetByServiceTypeIdAsync(serviceTypeId));

//         [HttpPost]
//         [Authorize]
//         public async Task<IActionResult> Create(Services service)
//         {
//             var created = await _service.CreateAsync(service);
//             return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(int id, Services service)
//         {
//             var updated = await _service.UpdateAsync(id, service);
//             return updated ? Ok() : NotFound();
//         }

//         [HttpDelete("{id}")]
//         [Authorize]
//         public async Task<IActionResult> Delete(int id)
//         {
//             var deleted = await _service.DeleteAsync(id);
//             return deleted ? Ok() : NotFound();
//         }
//     }

// }