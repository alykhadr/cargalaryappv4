using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Dtos.UserFavoriteAdmin.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : ControllerBase
    {
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
                return BadRequest(new ApiErrorResponse(UserNotFoundCode, StatusCodes.Status400BadRequest));
            }

            var favorites = await _favoritesService.GetAllAsync();
            var myFavorites = favorites.Where(x => x.UserId == userId).ToList();
            return Ok(myFavorites);
        }
    }
}
