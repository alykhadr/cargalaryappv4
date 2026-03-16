using CarGalary.Application.Dtos.ContactUs.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/contact-us")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactUsResponseDto>>> GetContactUs()
        {
            var items = await _contactUsService.GetAllAsync();
            return Ok(items);
        }
    }
}
