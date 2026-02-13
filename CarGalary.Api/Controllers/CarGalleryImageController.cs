
// using CarGalary.Application.Interfaces;
// using CarGalary.Domain.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize]
//     public class CarGalleryImageController : ControllerBase
//     {
//         private readonly ICarGalleryImageService _service;

//         public CarGalleryImageController(ICarGalleryImageService service)
//         {
//             _service = service;
//         }

//         [HttpPost]
//         public async Task<IActionResult> AddImage([FromBody] CarGalleryImage image)
//         {
//             var added = await _service.AddImageAsync(image);
//             return CreatedAtAction(nameof(GetImageById), new { imageId = added.ImageId }, added);
//         }

//         [HttpGet("{imageId}")]
//         [AllowAnonymous]
//         public async Task<IActionResult> GetImageById(int imageId)
//         {
//             var image = await _service.GetImageByIdAsync(imageId);
//             if (image == null) return NotFound();
//             return Ok(image);
//         }

//         [HttpGet("car/{carId}")]
//         [AllowAnonymous]
//         public async Task<IActionResult> GetImagesByCar(int carId)
//         {
//             var images = await _service.GetImagesByCarAsync(carId);
//             return Ok(images);
//         }

//         [HttpPut("{imageId}")]
//         public async Task<IActionResult> UpdateImage(int imageId, [FromBody] CarGalleryImage image)
//         {
//             if (imageId != image.ImageId) return BadRequest("ID mismatch");

//             var updated = await _service.UpdateImageAsync(image);
//             if (!updated) return NotFound();
//             return Ok();
//         }

//         [HttpDelete("{imageId}")]
//         public async Task<IActionResult> DeleteImage(int imageId)
//         {
//             var deleted = await _service.DeleteImageAsync(imageId);
//             if (!deleted) return NotFound();
//             return Ok();
//         }
//     }
// }