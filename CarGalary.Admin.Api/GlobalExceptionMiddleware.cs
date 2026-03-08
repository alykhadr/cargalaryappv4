using System.Text.Json;
using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.ErrorCatalog;

namespace CarGalary.Admin.Api
{
    public class GlobalExceptionMiddleware
    {
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
            var hasNumericMessage = !string.IsNullOrWhiteSpace(message) && message.All(char.IsDigit);
            var code = hasNumericMessage
                ? message
                : $"HTTP_{statusCode}";
            var baseMessage = string.IsNullOrWhiteSpace(message) ? code : message;

            var entry = _errorCatalogService.GetByCode(code);
            if (entry == null)
            {
                return new ApiErrorResponse(
                    baseMessage,
                    statusCode,
                    errorCode: code,
                    messageAr: baseMessage,
                    messageEn: baseMessage);
            }

            return new ApiErrorResponse(
                baseMessage,
                statusCode,
                errorCode: entry.ErrorCode,
                messageAr: entry.MessageAr,
                messageEn: entry.MessageEn);
        }
    }
}
