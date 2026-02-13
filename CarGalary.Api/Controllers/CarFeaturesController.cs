

// using CarGalary.Application.Dtos;
// using CarGalary.Application.Interfaces;
// using FluentValidation;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     [Authorize(Roles = "Admin,Manager")]
//     public class CarFeaturesController : ControllerBase
//     {

//         private readonly ICarFeatureService _service;
//         private readonly IValidator<AssignCarFeaturesDto> _validator;

//         public CarFeaturesController(
//             ICarFeatureService service,IValidator<AssignCarFeaturesDto> validator)
//         {
//             _service = service;
//              _validator = validator;
//         }

//         [HttpGet]
//         [AllowAnonymous]
//         public async Task<IActionResult> GetAll()
//         {
//             return Ok(await _service.GetAllAsync());
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] string name)
//         {
//             return Ok(await _service.CreateAsync(name));
//         }

//         [HttpPost("assign")]
//         public async Task<IActionResult> AssignFeatures(
//             [FromBody] AssignCarFeaturesDto dto)
//         {
//             var validation = await _validator.ValidateAsync(dto);
//             if (!validation.IsValid)
//             {
//                 var errors = validation.Errors
//                     .GroupBy(e => e.PropertyName)
//                     .ToDictionary(
//                         g => g.Key,
//                         g => g.Select(e => e.ErrorMessage).ToArray());

//                 return BadRequest(new { errors });
//             }

//             await _service.AssignFeaturesToCarAsync(dto.CarId, dto.FeatureIds);
//             return Ok(new { message = "Features assigned successfully" });
//         }
//     }
// }