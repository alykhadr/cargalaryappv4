using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/company-information")]
    public class CompanyInformationController : ControllerBase
    {
        private const string CompanyInfoNotFoundCode = "1304";

        private readonly ICompanyInformationService _service;

        public CompanyInformationController(ICompanyInformationService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            var company = (await _service.GetAllAsync()).FirstOrDefault();
            if (company == null)
            {
                return NotFound(new ApiErrorResponse(CompanyInfoNotFoundCode, StatusCodes.Status404NotFound));
            }

            return Ok(company);
        }
    }
}
