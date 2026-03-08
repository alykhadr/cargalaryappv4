using CarGalary.Application.Dtos.Auth;
using CarGalary.Application.ErrorCatalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarGalary.Admin.Api
{
    public class ApiErrorResponseFilter : IAsyncResultFilter
    {
        private readonly IErrorCatalogService _errorCatalogService;

        public ApiErrorResponseFilter(IErrorCatalogService errorCatalogService)
        {
            _errorCatalogService = errorCatalogService;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.Result = WrapErrorResult(context.Result);
            await next();
        }

        private IActionResult WrapErrorResult(IActionResult result)
        {
            if (result is ObjectResult objectResult && IsErrorStatusCode(objectResult.StatusCode))
            {
                if (objectResult.Value is ApiErrorResponse existingError)
                {
                    var enriched = EnrichErrorResponse(existingError, objectResult.StatusCode ?? StatusCodes.Status400BadRequest);
                    return new ObjectResult(enriched) { StatusCode = enriched.StatusCode };
                }

                var statusCode = objectResult.StatusCode ?? StatusCodes.Status400BadRequest;
                var (message, errors) = ExtractMessageAndErrors(objectResult.Value, statusCode);
                return new ObjectResult(EnrichErrorResponse(new ApiErrorResponse(message, statusCode, errors), statusCode))
                {
                    StatusCode = statusCode
                };
            }

            if (result is StatusCodeResult statusCodeResult && IsErrorStatusCode(statusCodeResult.StatusCode))
            {
                var message = GetDefaultMessage(statusCodeResult.StatusCode);
                return new ObjectResult(EnrichErrorResponse(new ApiErrorResponse(message, statusCodeResult.StatusCode), statusCodeResult.StatusCode))
                {
                    StatusCode = statusCodeResult.StatusCode
                };
            }

            if (result is ContentResult contentResult && IsErrorStatusCode(contentResult.StatusCode))
            {
                var statusCode = contentResult.StatusCode ?? StatusCodes.Status400BadRequest;
                var message = string.IsNullOrWhiteSpace(contentResult.Content)
                    ? GetDefaultMessage(statusCode)
                    : contentResult.Content;

                return new ObjectResult(EnrichErrorResponse(new ApiErrorResponse(message, statusCode), statusCode))
                {
                    StatusCode = statusCode
                };
            }

            return result;
        }

        private static bool IsErrorStatusCode(int? statusCode)
        {
            return statusCode.HasValue && statusCode.Value >= 400;
        }

        private static (string Message, List<string>? Errors) ExtractMessageAndErrors(object? value, int statusCode)
        {
            if (value == null)
            {
                return (GetDefaultMessage(statusCode), null);
            }

            if (value is string message)
            {
                return (message, null);
            }

            if (value is IEnumerable<string> errorsList)
            {
                var errorsObj = errorsList.Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
                return (errorsObj.FirstOrDefault() ?? GetDefaultMessage(statusCode), errorsObj.Count > 0 ? errorsObj : null);
            }

            var valueType = value.GetType();
            var messageProp = valueType.GetProperty("message") ?? valueType.GetProperty("Message");
            var errorProp = valueType.GetProperty("error") ?? valueType.GetProperty("Error");
            var errorsProp = valueType.GetProperty("errors") ?? valueType.GetProperty("Errors");

            var messageValue = messageProp?.GetValue(value)?.ToString()
                              ?? errorProp?.GetValue(value)?.ToString();

            List<string>? errors = null;
            var errorsValue = errorsProp?.GetValue(value);
            if (errorsValue is IEnumerable<string> enumerableErrors)
            {
                errors = enumerableErrors.Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            }

            var finalMessage = !string.IsNullOrWhiteSpace(messageValue)
                ? messageValue
                : (errors?.FirstOrDefault() ?? GetDefaultMessage(statusCode));

            return (finalMessage, errors);
        }

        private static string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Resource not found",
                StatusCodes.Status409Conflict => "Conflict",
                StatusCodes.Status500InternalServerError => "Internal server error",
                _ => "Request failed"
            };
        }

        private ApiErrorResponse EnrichErrorResponse(ApiErrorResponse response, int statusCode)
        {
            var code = ResolveCode(response.ErrorCode, response.Message, statusCode);
            var entry = _errorCatalogService.GetByCode(code);

            response.StatusCode = statusCode;
            response.ErrorCode = code;

            if (entry != null)
            {
                response.MessageAr = entry.MessageAr;
                response.MessageEn = entry.MessageEn;
            }
            else
            {
                response.MessageAr ??= response.Message;
                response.MessageEn ??= response.Message;
            }

            return response;
        }

        private static string ResolveCode(string? errorCode, string? message, int statusCode)
        {
            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                return errorCode.Trim();
            }

            if (!string.IsNullOrWhiteSpace(message) && message.All(char.IsDigit))
            {
                return message.Trim();
            }

            return $"HTTP_{statusCode}";
        }
    }
}
