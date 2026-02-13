

// using CarGalary.Application.Interfaces;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CarTypeController : ControllerBase
//     {
//         private readonly ICarTypeService carTypeService;

//         public CarTypeController(ICarTypeService carTypeService)
//         {
//             this.carTypeService = carTypeService;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<CarType>>> GetCarTypes()
//         {
//             return await carTypeService.GetCarTypes();
//         }


//         [HttpGet("{id}")]
//         public async Task<ActionResult<CarType>> GetCarTypeById(int id)
//         {
//             var brand = await carTypeService.GetCarTypeById(id);

//             if (brand is null)
//                 return NotFound();

//             return brand;
//         }

//          [Authorize(Roles="Admin,Manager")]
//         [HttpPost]
//         public async Task<ActionResult<CarType>> PostCarType(CarType type)
//         {
           
//           await  carTypeService.CreateCarType(type);
//             return CreatedAtAction(nameof(GetCarTypeById), new { id = type.Id }, type);
//         }


//          [Authorize(Roles="Admin,Manager")]
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutCarType(int id, CarType type)
//         {
//              if (id != type.Id)
//                 return BadRequest();
//                var result=  await carTypeService.UpdateCarType(id,type);
//                if (!result) return BadRequest();
//             return Ok();
//         }

//          [Authorize(Roles="Admin,Manager")]
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteCarType(int id)
//         {
//             var result = await carTypeService.DeleteCarTypeById(id);
//             if (!result) return NotFound();
//             return Ok();
//         }
//     }
// }