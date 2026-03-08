namespace CarGalary.Application.Dtos.Auth
{
    public class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorCode { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }

        public ApiErrorResponse() { }

        public ApiErrorResponse(
            string message,
            int statusCode = 400,
            List<string>? errors = null,
            string? errorCode = null,
            string? messageAr = null,
            string? messageEn = null)
        {
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
            ErrorCode = errorCode;
            MessageAr = messageAr;
            MessageEn = messageEn;
        }
    }
}
