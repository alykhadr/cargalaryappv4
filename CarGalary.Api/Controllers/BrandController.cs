

using CarGalary.Application.Dtos.Brand;
using CarGalary.Application.Dtos.CarModel.Query;
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

        public BrandController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await brandService.GetAllAsync();
            return Ok(brands);
        }


        [HttpGet("{brandId:int}/models")]
        public async Task<ActionResult<IEnumerable<CarModelByBrandResponseDto>>> GetModelsByBrand(int brandId)
        {
            var models = await brandService.GetCarModelsByBrandAsync(brandId);
            return Ok(models);
        }


    }
}
