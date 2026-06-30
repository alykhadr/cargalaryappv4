using CarGalary.Application.Dtos.Offer.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/offer")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffers()
        {
            var now = DateTime.UtcNow;
            var offers = (await _offerService.GetAllAsync())
                .Where(x => x.IsAvailable && (!x.ExpiredAt.HasValue || x.ExpiredAt.Value >= now))
                .ToList();
            return Ok(offers);
        }
    }
}
