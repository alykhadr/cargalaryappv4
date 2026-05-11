using CarGalary.Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CarGalary.Api.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private const string DefaultBadRequestCode = "1101";
        protected BadRequestObjectResult BadRequestErrorResponse(
            string errorCode = DefaultBadRequestCode,
            IEnumerable<string>? errors = null)
        {
            return BadRequest(new ApiErrorResponse(
                errorCode,
                StatusCodes.Status400BadRequest,
                errors?.ToList()));
        }

        protected NotFoundObjectResult NotFoundErrorResponse(string errorCode)
        {
            return NotFound(new ApiErrorResponse(errorCode, StatusCodes.Status404NotFound));
        }

        protected static string ResolveBadRequestCode(Exception exception, string fallbackCode = DefaultBadRequestCode)
        {
            return !string.IsNullOrWhiteSpace(exception.Message) && exception.Message.All(char.IsDigit)
                ? exception.Message
                : fallbackCode;
        }
    }
}
