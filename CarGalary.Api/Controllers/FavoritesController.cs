using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Command;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;
using CarGalary.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : ApiControllerBase
    {
        private const string ValidationFailedCode = "1101";
        private const string UserNotFoundCode = "1227";

        private readonly IFavoritesService _favoritesService;
        private readonly ICurrentUserService _currentUserService;

        public FavoritesController(
            IFavoritesService favoritesService,
            ICurrentUserService currentUserService)
        {
            _favoritesService = favoritesService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavoriteAdminResponseDto>>> GetFavorites()
        {
            var favorites = await _favoritesService.GetAllAsync();
            return Ok(favorites);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<UserFavoriteAdminResponseDto>>> GetMyFavorites()
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            var favorites = await _favoritesService.GetAllAsync();
            var myFavorites = favorites.Where(x => x.UserId == userId).ToList();
            return Ok(myFavorites);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFavorite(
            [FromBody] CreateMyFavoriteRequest request,
            [FromServices] IValidator<CreateUserFavoriteAdminRequestDto> validator)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            var dto = new CreateUserFavoriteAdminRequestDto
            {
                UserId = userId,
                CarId = request.CarId,
                Notes = request.Notes,
                Priority = request.Priority
            };

            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequestErrorResponse(
                    ValidationFailedCode,
                    validation.Errors.Select(e => e.ErrorMessage));
            }

            var existing = await _favoritesService.GetByIdAsync(userId, request.CarId);
            if (existing != null)
            {
                return BadRequestErrorResponse(errors: new[] { "Favorite already exists" });
            }

            var created = await _favoritesService.CreateAsync(dto);
            return Ok(created);
        }

        public class CreateMyFavoriteRequest
        {
            public int CarId { get; set; }
            public string? Notes { get; set; }
            public int Priority { get; set; }
        }
    }
}
