using System.Net;
using System.Text.Json;
using CarRental.ntier.BLL.Exceptions;

namespace CarRental.ntier.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorDetails errorDetails;
            int statusCode;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorDetails = CreateErrorDetails(statusCode, notFoundEx.Message, notFoundEx.ToString(), context.TraceIdentifier);
                    break;

                case BadRequestException badRequestEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = CreateErrorDetails(statusCode, badRequestEx.Message, badRequestEx.ToString(), context.TraceIdentifier);
                    break;

                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = CreateErrorDetails(
                        statusCode,
                        "Validation failed",
                        validationEx.ToString(),
                        context.TraceIdentifier,
                        validationEx.Errors
                    );
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorDetails = CreateErrorDetails(
                        statusCode,
                        _env.IsDevelopment() ? exception.Message : "An internal server error occurred",
                        _env.IsDevelopment() ? exception.StackTrace : null,
                        context.TraceIdentifier
                    );
                    break;
            }

            context.Response.StatusCode = statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(errorDetails, options);
            await context.Response.WriteAsync(json);
        }

        private ErrorDetails CreateErrorDetails(
            int statusCode,
            string message,
            string? details,
            string traceId,
            Dictionary<string, string[]>? errors = null)
        {
            return new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
                Details = _env.IsDevelopment() ? details : null,
                TraceId = traceId,
                Errors = errors
            };
        }
    }
}
