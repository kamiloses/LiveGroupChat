using System.Net;
using System.Text.Json;

namespace LiveGroupChat.Exceptions;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidUserIdException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = ex.Message });
        return context.Response.WriteAsync(result);
    }
}
