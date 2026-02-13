
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;


// [Route("api/[controller]")]
// [ApiController]
// [Authorize]
// public class CarColorController : ControllerBase
// {
//     private readonly ICarColorService _service;

//     public CarColorController(ICarColorService service)
//     {
//         _service = service;
//     }

//     [HttpPost]
//     public async Task<IActionResult> AddCarColor([FromBody] CarColor carColor)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);

//         var added = await _service.AddCarColorAsync(carColor);
//         return CreatedAtAction(nameof(GetCarColorById), new { id = added.Id }, added);
//     }

//     [HttpGet("{id}")]
//     [AllowAnonymous]
//     public async Task<IActionResult> GetCarColorById(int id)
//     {
//         var color = await _service.GetCarColorByIdAsync(id);
//         if (color == null) return NotFound();
//         return Ok(color);
//     }

//     [HttpGet]
//     [AllowAnonymous]
//     public async Task<IActionResult> GetAllColors()
//     {
//         var colors = await _service.GetAllCarColorsAsync();
//         return Ok(colors);
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateCarColor(int id, [FromBody] CarColor carColor)
//     {
//         if (id != carColor.Id) return BadRequest("ID mismatch");

//         var updated = await _service.UpdateCarColorAsync(carColor);
//         if (!updated) return NotFound();

//         return Ok();
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteCarColor(int id)
//     {
//         var deleted = await _service.DeleteCarColorAsync(id);
//         if (!deleted) return NotFound();

//         return Ok();
//     }
// }
