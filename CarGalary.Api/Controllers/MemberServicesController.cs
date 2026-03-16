using CarGalary.Application.Dtos.MemberService.Query;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/member-services")]
    public class MemberServicesController : ControllerBase
    {
        private readonly IMemberServiceService _memberServiceService;

        public MemberServicesController(IMemberServiceService memberServiceService)
        {
            _memberServiceService = memberServiceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberServiceResponseDto>>> GetMemberServices()
        {
            var memberServices = await _memberServiceService.GetAllAsync();
            return Ok(memberServices);
        }
    }
}
