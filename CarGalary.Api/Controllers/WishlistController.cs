using System.Globalization;
using System.Security.Claims;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/wishlist")]
    public class WishlistController : ApiControllerBase
    {
        private const string UserNotFoundCode = "1227";
        private const string CarNotFoundCode = "1209";

        private readonly IFrontendApiService _frontendApiService;

        public WishlistController(IFrontendApiService frontendApiService)
        {
            _frontendApiService = frontendApiService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetWishlist()
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            var items = await _frontendApiService.GetWishlistAsync(userId);
            return Ok(new { items });
        }

        [HttpGet("{carId}/status")]
        public async Task<ActionResult<object>> GetWishlistStatus(string carId)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            if (!TryParseCarId(carId, out var parsedCarId))
            {
                return BadRequestErrorResponse(CarNotFoundCode);
            }

            var isWishlisted = await _frontendApiService.IsWishlistedAsync(userId, parsedCarId);
            return Ok(new { isWishlisted });
        }

        [HttpPost("{carId}")]
        public async Task<ActionResult<object>> AddToWishlist(string carId)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            if (!TryParseCarId(carId, out var parsedCarId))
            {
                return BadRequestErrorResponse(CarNotFoundCode);
            }

            await _frontendApiService.AddToWishlistAsync(userId, parsedCarId);
            return Ok(new { success = true });
        }

        [HttpDelete("{carId}")]
        public async Task<ActionResult<object>> RemoveFromWishlist(string carId)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return BadRequestErrorResponse(UserNotFoundCode);
            }

            if (!TryParseCarId(carId, out var parsedCarId))
            {
                return BadRequestErrorResponse(CarNotFoundCode);
            }

            await _frontendApiService.RemoveFromWishlistAsync(userId, parsedCarId);
            return Ok(new { success = true });
        }

        private bool TryGetCurrentUserId(out Guid userId)
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out userId);
        }

        private static bool TryParseCarId(string carId, out int parsedCarId)
        {
            return int.TryParse(carId, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedCarId) &&
                parsedCarId > 0;
        }
    }
}
