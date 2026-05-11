using System.Text.Json;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.ErrorCatalog;

namespace CarGalary.Admin.Api
{
    public class GlobalExceptionMiddleware
    {
        private const string ValidationFailedCode = "1101";
        private const string InternalServerErrorCode = "1102";

        private readonly RequestDelegate _next;
        private readonly IErrorCatalogService _errorCatalogService;

        public GlobalExceptionMiddleware(RequestDelegate next, IErrorCatalogService errorCatalogService)
        {
            _next = next;
            _errorCatalogService = errorCatalogService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = BuildErrorResponse(ex.Message, statusCode);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private ApiErrorResponse BuildErrorResponse(string message, int statusCode)
        {
            var code = ResolveErrorCode(message, statusCode);

            var entry = _errorCatalogService.GetByCode(code);
            if (entry == null)
            {
                return new ApiErrorResponse(
                    code,
                    statusCode,
                    errorCode: code,
                    messageAr: code,
                    messageEn: code);
            }

            return new ApiErrorResponse(
                code,
                statusCode,
                errorCode: entry.ErrorCode,
                messageAr: entry.MessageAr,
                messageEn: entry.MessageEn);
        }

        private static string ResolveErrorCode(string message, int statusCode)
        {
            if (!string.IsNullOrWhiteSpace(message) && message.All(char.IsDigit))
            {
                return message;
            }

            return statusCode switch
            {
                StatusCodes.Status400BadRequest => ValidationFailedCode,
                StatusCodes.Status500InternalServerError => InternalServerErrorCode,
                _ => $"HTTP_{statusCode}"
            };
        }
    }
}
