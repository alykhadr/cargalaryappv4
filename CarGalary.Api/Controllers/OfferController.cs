
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize]
//     public class OfferController : ControllerBase
//     {
//         private readonly IOfferService _service;

//         public OfferController(IOfferService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var offers = await _service.GetAllAsync();
//             return Ok(offers);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> Get(int id)
//         {
//             var offer = await _service.GetByIdAsync(id);
//             if (offer == null) return NotFound();
//             return Ok(offer);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] Offer offer)
//         {
//             var created = await _service.CreateAsync(offer);
//             return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(int id, [FromBody] Offer offer)
//         {
//             if (id != offer.Id) return BadRequest();

//             var updated = await _service.UpdateAsync(offer);
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