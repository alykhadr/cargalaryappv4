using CarGalary.Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarGalary.Admin.Api
{
    public class ApiErrorResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.Result = WrapErrorResult(context.Result);
            await next();
        }

        private static IActionResult WrapErrorResult(IActionResult result)
        {
            if (result is ObjectResult objectResult && IsErrorStatusCode(objectResult.StatusCode))
            {
                if (objectResult.Value is ApiErrorResponse)
                {
                    return result;
                }

                var statusCode = objectResult.StatusCode ?? StatusCodes.Status400BadRequest;
                var (message, errors) = ExtractMessageAndErrors(objectResult.Value, statusCode);
                return new ObjectResult(new ApiErrorResponse(message, statusCode, errors))
                {
                    StatusCode = statusCode
                };
            }

            if (result is StatusCodeResult statusCodeResult && IsErrorStatusCode(statusCodeResult.StatusCode))
            {
                var message = GetDefaultMessage(statusCodeResult.StatusCode);
                return new ObjectResult(new ApiErrorResponse(message, statusCodeResult.StatusCode))
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

                return new ObjectResult(new ApiErrorResponse(message, statusCode))
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
    }
}
