using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LookupsController : ControllerBase
    {
        private readonly ILookupDetailsService _lookupDetailsService;

        public LookupsController(ILookupDetailsService lookupDetailsService)
        {
            _lookupDetailsService = lookupDetailsService;
        }

        [HttpGet("{masterCode}")]
        
        public async Task<IActionResult> GetByMasterCode(string masterCode)
        {
            var data = await _lookupDetailsService.GetByMasterCodeAsync(masterCode);
            return Ok(data);
        }
    }
}
