using CarGalary.Application.Dtos.Brand.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBrandRequestDto createBrandRequestDto,
            [FromServices] IValidator<CreateBrandRequestDto> validator)
        {
            var validationResult = validator.Validate(createBrandRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var created = await _brandService.CreateAsync(createBrandRequestDto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateBrandRequestDto updateBrandRequestDto,
            [FromServices] IValidator<UpdateBrandRequestDto> validator)
        {
            var validationResult = validator.Validate(updateBrandRequestDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                await _brandService.UpdateAsync(id, updateBrandRequestDto);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex.Message == "Brand not found")
            {
                return NotFound();
            }
        }
    }
}
