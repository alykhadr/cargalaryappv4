using CarGalary.Application.Dtos.PrivacyPolicy.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/privacy-policy")]
    public class PrivacyPolicyController : ControllerBase
    {
        private readonly IPrivacyPolicyService _privacyPolicyService;

        public PrivacyPolicyController(IPrivacyPolicyService privacyPolicyService)
        {
            _privacyPolicyService = privacyPolicyService;
        }

        [HttpGet]
        public async Task<ActionResult<PrivacyPolicyResponseDto>> GetPrivacyPolicy()
        {
            var privacyPolicy = await _privacyPolicyService.GetFirstAsync();
            if (privacyPolicy == null)
            {
                return NotFound();
            }

            return Ok(privacyPolicy);
        }
    }
}
