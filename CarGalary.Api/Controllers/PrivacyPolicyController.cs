using CarGalary.Application.Dtos.PrivacyPolicy.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/privacy-policy")]
    public class PrivacyPolicyController : ApiControllerBase
    {
        private const string PrivacyPolicyNotFoundCode = "1335";

        private readonly IPrivacyPolicyService _service;

        public PrivacyPolicyController(IPrivacyPolicyService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PrivacyPolicyResponseDto>> GetLatest()
        {
            var privacyPolicy = await _service.GetLatestAvailableAsync();
            if (privacyPolicy == null)
            {
                return NotFoundErrorResponse(PrivacyPolicyNotFoundCode);
            }

            return Ok(privacyPolicy);
        }
    }
}
