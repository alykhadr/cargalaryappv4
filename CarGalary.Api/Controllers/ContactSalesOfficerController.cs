using CarGalary.Application.Dtos.ContactSalesOfficer.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/contact-sales")]
    public class ContactSalesOfficerController : ControllerBase
    {
        private readonly IContactSalesOfficerService _contactSalesOfficerService;

        public ContactSalesOfficerController(IContactSalesOfficerService contactSalesOfficerService)
        {
            _contactSalesOfficerService = contactSalesOfficerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactSalesOfficerResponseDto>>> GetContactSales()
        {
            var items = await _contactSalesOfficerService.GetAllAsync();
            return Ok(items);
        }
    }
}
