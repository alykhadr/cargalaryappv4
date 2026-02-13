

// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     [Authorize]
//     public class ContactSalesOfficerController : ControllerBase
//     {
//         private readonly IContactSalesOfficerService _service;

//         public ContactSalesOfficerController(IContactSalesOfficerService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//             => Ok(await _service.GetAllAsync());

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(int id)
//         {
//             var item = await _service.GetByIdAsync(id);
//             if (item == null) return NotFound();
//             return Ok(item);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create(ContactSalesOfficer model)
//         {
//             var created = await _service.CreateAsync(model);
//             return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(int id, ContactSalesOfficer model)
//         {
//             if (!await _service.UpdateAsync(id, model))
//                 return NotFound();
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(int id)
//         {
//             if (!await _service.DeleteAsync(id))
//                 return NotFound();
//             return NoContent();
//         }
//     }
// }