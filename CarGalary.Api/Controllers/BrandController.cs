

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


        // [HttpGet("{id}")]
        // public async Task<ActionResult<CarBrand>> GetBrand(int id)
        // {
        //     var brand = await brandService.GetBrandById(id);

        //     if (brand is null)
        //         return NotFound();

        //     return Ok(brand);
        // }

       // [Authorize(Roles = "Admin,Manager")]
        [HttpPost]

        public async Task<ActionResult<CarBrand>> PostBrand(BrandDto brandDto)
        {

            await brandService.CreateAsync(brandDto);
            //return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brand);
            return Ok();
        }


        // [Authorize(Roles="Admin,Manager")]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutBrand(int id, CarBrand brand)
        // {
        //     if (id != brand.Id)
        //         return BadRequest();


        //     var result = await brandService.UpdateBrand(id, brand);
        //     if (!result) return BadRequest();
        //     return Ok();
        // }

        // [Authorize(Roles="Admin,Manager")]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteBrand(int id)
        // {
        //     var result = await brandService.DeleteBrandById(id);
        //     if (!result) return NotFound();
        //     return Ok();
        // }
    }
}