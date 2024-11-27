using System.Net;

namespace MyAirbnb.API.Middlewares;

public class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILoggerFactory _loggerFactory;

    public GlobalExceptionMiddleware(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var logger = _loggerFactory.CreateLogger(typeof(GlobalExceptionMiddleware));

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception _)
    {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync("Unable to process your request at the moment!");
    }
}
