using CarGalary.Application.Dtos.FAQ.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/faqs")]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQsController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FAQResponseDto>>> GetFAQs()
        {
            var items = await _faqService.GetAllAsync();
            return Ok(items);
        }
    }
}
