

// using CarGalary.Application.Interfaces;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class ContactUsController : ControllerBase
//     {
//         private readonly IContactUsService _service;

//         public ContactUsController(IContactUsService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var contacts = await _service.GetAllAsync();
//             return Ok(contacts);
//         }
//     }
// }