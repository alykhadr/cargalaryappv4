using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteController : ControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string UserFavoriteNotFoundCode = "1319";

        private readonly IFavoritesService _service;

        public UserFavoriteController(IFavoritesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{userId:guid}/{carId:int}")]
        public async Task<IActionResult> GetById(Guid userId, int carId)
        {
            var item = await _service.GetByIdAsync(userId, carId);
            return item == null
                ? NotFound(new ApiErrorResponse(UserFavoriteNotFoundCode, StatusCodes.Status404NotFound))
                : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserFavoriteAdminRequestDto dto,
            [FromServices] IValidator<CreateUserFavoriteAdminRequestDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(new ApiErrorResponse(
                    ValidationFailedCode,
                    StatusCodes.Status400BadRequest,
                    validation.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{userId:guid}/{carId:int}")]
        public async Task<IActionResult> Update(
            Guid userId,
            int carId,
            [FromBody] UpdateUserFavoriteAdminRequestDto dto,
            [FromServices] IValidator<UpdateUserFavoriteAdminRequestDto> validator)
        {
            var existing = await _service.GetByIdAsync(userId, carId);
            if (existing == null)
            {
                return NotFound(new ApiErrorResponse(UserFavoriteNotFoundCode, StatusCodes.Status404NotFound));
            }

            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(new ApiErrorResponse(
                    ValidationFailedCode,
                    StatusCodes.Status400BadRequest,
                    validation.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                await _service.UpdateAsync(userId, carId, dto);
                return Ok();
            }
            catch (Exception e) when (e.Message == "UserFavorite not found")
            {
                return NotFound(new ApiErrorResponse(UserFavoriteNotFoundCode, StatusCodes.Status404NotFound));
            }
        }

        [HttpDelete("{userId:guid}/{carId:int}")]
        public async Task<IActionResult> Delete(Guid userId, int carId)
        {
            var existing = await _service.GetByIdAsync(userId, carId);
            if (existing == null)
            {
                return NotFound(new ApiErrorResponse(UserFavoriteNotFoundCode, StatusCodes.Status404NotFound));
            }

            try
            {
                await _service.DeleteAsync(userId, carId);
                return Ok();
            }
            catch (Exception e) when (e.Message == "UserFavorite not found")
            {
                return NotFound(new ApiErrorResponse(UserFavoriteNotFoundCode, StatusCodes.Status404NotFound));
            }
        }
    }
}
