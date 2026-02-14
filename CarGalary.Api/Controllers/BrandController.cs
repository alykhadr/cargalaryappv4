

using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService brandService;
        private readonly ILogger<BrandController> _logger;

        public BrandController(IBrandService brandService,ILogger<BrandController> logger)
        {
            this._logger = logger;
            this.brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            try
            {
            
            
            var brands = await brandService.GetAllAsync();
            return Ok(brands);
            }
             catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,ex);

            }
           
        }


    }
}