
using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
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
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchesController(IBranchService service)
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

        [HttpGet("{id}")]
        [PermissionAuthorize("branches.view")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                return Ok(branch);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }

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
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }
                var response = await _service.CreateAsync(createBrancRequestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("branches.edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchRequestDto updateBranchRequestDto,
         [FromServices] IValidator<UpdateBranchRequestDto> _validator)
        {
            try
            {
                var validator = _validator.Validate(updateBranchRequestDto);
                if (!validator.IsValid)
                {
                    var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }
                var updated = await _service.UpdateAsync(id, updateBranchRequestDto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("branches.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
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
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        
    }
}