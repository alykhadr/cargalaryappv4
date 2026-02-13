

// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize]
//     public class CompanyInformationController : ControllerBase
//     {
//         private readonly ICompanyInformationService _service;

//         public CompanyInformationController(ICompanyInformationService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var companies = await _service.GetAllAsync();
//             return Ok(companies);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> Get(int id)
//         {
//             var company = await _service.GetByIdAsync(id);
//             if (company == null) return NotFound();
//             return Ok(company);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] CompanyInformation company)
//         {
//             var created = await _service.CreateAsync(company);
//             return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(int id, [FromBody] CompanyInformation company)
//         {
//             if (id != company.Id) return BadRequest();

//             var updated = await _service.UpdateAsync(company);
//             if (!updated) return NotFound();

//             return Ok();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(int id)
//         {
//             var deleted = await _service.DeleteAsync(id);
//             if (!deleted) return NotFound();

//             return Ok();
//         }
//     }
// }