
// using System.Security.Claims;
// using CarGalary.Application.Dtos;
// using CarGalary.Application.Interfaces;
// using FluentValidation;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace CarGalary.Api.Controllers
// {
//    [ApiController]
// [Route("api/[controller]")]
// [Authorize]
// public class FavoritesController : ControllerBase
// {
 
//     private readonly IFavoritesService _favoritesService;
//      private readonly IValidator<AddFavoriteDto> _validator;

//     public FavoritesController(
//         IFavoritesService favoritesService,
//         IValidator<AddFavoriteDto> validator)
//     {
//         _favoritesService = favoritesService;
//         _validator = validator;
//     }

//     private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

//     // GET: api/favorites
//     [HttpGet]
//     public async Task<IActionResult> GetMyFavorites()
//     {
//         var favorites = await _favoritesService.GetMyFavoritesAsync(UserId);
//         return Ok(favorites);
//     }

//     //POST: api/favorites
//     [HttpPost]
//     public async Task<IActionResult> AddToFavorites([FromBody] AddFavoriteDto dto)
//     {
//         var validation = await _validator.ValidateAsync(dto);
//         if (!validation.IsValid)
//         {
//             var errors = validation.Errors
//                 .GroupBy(e => e.PropertyName)
//                 .ToDictionary(
//                     g => g.Key,
//                     g => g.Select(e => e.ErrorMessage).ToArray()
//                 );

//             return BadRequest(new { errors });
//         }

//         await _favoritesService.AddToFavoritesAsync(UserId, dto.CarId);
//         return Ok(new { message = "Car added to favorites" });
//     }

//     // DELETE: api/favorites/{carId}
//     [HttpDelete("{carId}")]
//     public async Task<IActionResult> RemoveFromFavorites(int carId)
//     {
//         await _favoritesService.RemoveFromFavoritesAsync(UserId, carId);
//         return Ok(new { message = "Car removed from favorites" });
//     }
// }
// }