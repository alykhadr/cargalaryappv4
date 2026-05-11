using CarGalary.Application.Dtos.Branch;
using CarGalary.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    [ApiController]
    [Route("api/branches")]
    public class BranchesController : ApiControllerBase
    {
        private const string BranchNotFoundCode = "1301";

        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchResponseDto>>> GetBranches()
        {
            var branches = await _branchService.GetAllAsync();
            return Ok(branches);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BranchResponseDto>> GetBranchById(int id)
        {
            try
            {
                var branch = await _branchService.GetByIdAsync(id);
                return Ok(branch);
            }
            catch (Exception ex) when (ex.Message == "Branch not found")
            {
                return NotFoundErrorResponse(BranchNotFoundCode);
            }
        }
    }
}
