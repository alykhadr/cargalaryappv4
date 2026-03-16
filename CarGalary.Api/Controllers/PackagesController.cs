using CarGalary.Application.Dtos.Package.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageResponseDto>>> GetPackages()
        {
            var packages = await _packageService.GetAllAsync();
            return Ok(packages);
        }
    }
}
