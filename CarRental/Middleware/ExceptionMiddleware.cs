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

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
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
            var response = context.Response;

            var errorDetails = new ErrorDetails();

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = notFoundEx.Message,
                        Details = _env.IsDevelopment() ? notFoundEx.ToString() : null
                    };
                    break;

                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = badRequestEx.Message,
                        Details = _env.IsDevelopment() ? badRequestEx.ToString() : null
                    };
                    break;

                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Validation failed",
                        Details = _env.IsDevelopment() ? validationEx.ToString() : null,
                        Errors = validationEx.Errors
                    };
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Unauthorized access"
                    };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorDetails = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = _env.IsDevelopment() ? exception.Message : "An internal server error occurred",
                        Details = _env.IsDevelopment() ? exception.StackTrace : null
                    };
                    break;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(errorDetails, options);

            await context.Response.WriteAsync(json);
        }
    }
}
