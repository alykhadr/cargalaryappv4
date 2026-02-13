
using CarGalary.Application.Dtos.Branch.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
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

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Update(int id, [FromBody] Branch branch)
        // {
        //     if (id != branch.Id) return BadRequest();

        //     var updated = await _service.UpdateAsync(branch);
        //     if (!updated) return NotFound();

        //     return Ok();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var deleted = await _service.DeleteAsync(id);
        //     if (!deleted) return NotFound();

        //     return Ok();
        // }

        // [HttpGet("{branchId}/working-days")]
        // public async Task<IActionResult> GetWorkingDays(int branchId)
        // {
        //     var days = await _service.GetWorkingDaysAsync(branchId);
        //     return Ok(days);
        // }

        // [HttpPost("{branchId}/working-days")]
        // public async Task<IActionResult> AddWorkingDay(int branchId, [FromBody] BranchWorkingDays workingDay)
        // {
        //     var created = await _service.AddWorkingDayAsync(branchId, workingDay);
        //     return CreatedAtAction(nameof(GetWorkingDays), new { branchId }, created);
        // }

        // [HttpPut("working-days/{id}")]
        // public async Task<IActionResult> UpdateWorkingDay(int id, [FromBody] BranchWorkingDays workingDay)
        // {
        //     if (id != workingDay.Id) return BadRequest();

        //     var updated = await _service.UpdateWorkingDayAsync(workingDay);
        //     if (!updated) return NotFound();

        //     return NoContent();
        // }

        // [HttpDelete("working-days/{id}")]
        // public async Task<IActionResult> DeleteWorkingDay(int id)
        // {
        //     var deleted = await _service.DeleteWorkingDayAsync(id);
        //     if (!deleted) return NotFound();

        //     return NoContent();
        // }
    }
}