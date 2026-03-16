using CarGalary.Application.Dtos.Services.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicesResponseDto>>> GetServices()
        {
            var services = await _servicesService.GetAllAsync();
            return Ok(services);
        }
    }
}
