
using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Branch.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _service.GetAllAsync();
            return Ok(branches);
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> Get(int id)
        // {
        //     var branch = await _service.GetByIdAsync(id);
        //     if (branch == null) return NotFound();
        //     return Ok(branch);
        // }

        [HttpPost]
         [PermissionAuthorize("branches.create")]
        public async Task<IActionResult> Create([FromBody] CreateBrancRequestDto createBrancRequestDto,
         [FromServices] IValidator<CreateBrancRequestDto> _validator)
        {
            try
            {
                var validator = _validator.Validate(createBrancRequestDto);
                if (!validator.IsValid)
                {
                    // Return all errors as an array of strings
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _service.CreateAsync(createBrancRequestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("active")]
 [PermissionAuthorize("branches.active")]
        public async Task<IActionResult> Active(UpdateBranchWorkingDayRequestDto updateBranchWorkingDayRequestDto)
        {

            try
            {
                var updated = await _service.ActiveAsync(updateBranchWorkingDayRequestDto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        
    }
}