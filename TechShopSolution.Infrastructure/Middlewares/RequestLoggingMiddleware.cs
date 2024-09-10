using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace TechShopSolution.Infrastructure.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture request body
        var request = context.Request;
        var requestBody = string.Empty;

        if (request.ContentLength > 0 && request.Body.CanRead)
        {
            request.EnableBuffering(); // Enable buffering to read the body stream

            using (var reader = new StreamReader(request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0; // Reset the stream position
            }
        }

        // Log request details
        Log.Information("Request: Method={Method}, Path={Path}, QueryString={QueryString}, Headers={Headers}, Body={Body}",
            request.Method,
            request.Path,
            request.QueryString,
            request.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value)),
            requestBody);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log exceptions
            Log.Error(ex, "An unhandled exception occurred.");
            throw;
        }

        // Log response details
        Log.Information("Response: StatusCode={StatusCode}", context.Response.StatusCode);
    }
}
