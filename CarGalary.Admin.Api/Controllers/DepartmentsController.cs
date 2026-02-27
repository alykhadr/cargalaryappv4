using CarGalary.Admin.Api.Security;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.Department.Command;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [PermissionAuthorize("departments.view")]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("departments.view")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var department = await _departmentService.GetByIdAsync(id);
                return Ok(department);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }

        [HttpPost]
        [PermissionAuthorize("departments.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDepartmentRequestDto request,
            [FromServices] IValidator<CreateDepartmentRequestDto> validator)
        {
            try
            {
                var validation = validator.Validate(request);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var response = await _departmentService.CreateAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("departments.edit")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDepartmentRequestDto request,
            [FromServices] IValidator<UpdateDepartmentRequestDto> validator)
        {
            try
            {
                var validation = validator.Validate(request);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiErrorResponse("Validation failed", StatusCodes.Status400BadRequest, errors));
                }

                var result = await _departmentService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("departments.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _departmentService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }
    }
}
