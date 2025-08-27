using System.Net;
using System.Text.Json;
using CarRental.ntier.BLL.Exceptions;

namespace CarRental.ntier.API.Middleware;
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
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.NotFound);
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.BadRequest);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed: {Message}", ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.BadRequest, ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private async Task WriteErrorResponse(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode,
        Dictionary<string, string[]>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var message = exception is ValidationException ? "Validation failed" : exception.Message;

        var errorDetails = new ErrorDetails
        {
            StatusCode = (int)statusCode,
            Message = message,
            Details = _env.IsDevelopment() ? exception.ToString() : null,
            TraceId = context.TraceIdentifier,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(errorDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
