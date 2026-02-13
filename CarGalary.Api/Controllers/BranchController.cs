
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize]
//     public class BranchController : ControllerBase
//     {
//         private readonly IBranchService _service;

//         public BranchController(IBranchService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var branches = await _service.GetAllAsync();
//             return Ok(branches);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> Get(int id)
//         {
//             var branch = await _service.GetByIdAsync(id);
//             if (branch == null) return NotFound();
//             return Ok(branch);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] Branch branch)
//         {
//             var created = await _service.CreateAsync(branch);
//             return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(int id, [FromBody] Branch branch)
//         {
//             if (id != branch.Id) return BadRequest();

//             var updated = await _service.UpdateAsync(branch);
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

//         [HttpGet("{branchId}/working-days")]
//         public async Task<IActionResult> GetWorkingDays(int branchId)
//         {
//             var days = await _service.GetWorkingDaysAsync(branchId);
//             return Ok(days);
//         }

//         [HttpPost("{branchId}/working-days")]
//         public async Task<IActionResult> AddWorkingDay(int branchId, [FromBody] BranchWorkingDays workingDay)
//         {
//             var created = await _service.AddWorkingDayAsync(branchId, workingDay);
//             return CreatedAtAction(nameof(GetWorkingDays), new { branchId }, created);
//         }

//         [HttpPut("working-days/{id}")]
//         public async Task<IActionResult> UpdateWorkingDay(int id, [FromBody] BranchWorkingDays workingDay)
//         {
//             if (id != workingDay.Id) return BadRequest();

//             var updated = await _service.UpdateWorkingDayAsync(workingDay);
//             if (!updated) return NotFound();

//             return NoContent();
//         }

//         [HttpDelete("working-days/{id}")]
//         public async Task<IActionResult> DeleteWorkingDay(int id)
//         {
//             var deleted = await _service.DeleteWorkingDayAsync(id);
//             if (!deleted) return NotFound();

//             return NoContent();
//         }
//     }
// }