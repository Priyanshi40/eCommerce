using System.Net;
using System.Text.Json;
using BLL.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BLL.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await HandleExceptionAsync(context, ex.Message, ex.StatusCode);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, "You are not authorized to perform this action.", (int)HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            Console.WriteLine("---> Handled From Here!!");
            await HandleExceptionAsync(context, ex.Message, (int)HttpStatusCode.InternalServerError);
            // await HandleExceptionAsync(context, "An unexpected error occurred.", (int)HttpStatusCode.InternalServerError);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        string result = JsonSerializer.Serialize(new
        {
            success = false,
            message,
        });

        await context.Response.WriteAsync(result);
    }
}
