using Hotel_Booking_App.ExceptionHanlder;
using System.Text.Json;
namespace HotelBookingAPI.ExceptionHandlerGlobalExceptionMiddleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // continue pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred."); // ✅ Log the error
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message = exception.Message;

        switch (exception)
        {
            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                break;

            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case ConflictException:
                statusCode = StatusCodes.Status409Conflict;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        var result = JsonSerializer.Serialize(new
        {
            message,
            status = statusCode
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}
