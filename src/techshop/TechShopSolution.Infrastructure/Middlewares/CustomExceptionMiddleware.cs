using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TechShopSolution.Application.Models.Common;

namespace TechShopSolution.Infrastructure.Middlewares;

public class CustomUnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public CustomUnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Tiếp tục với pipeline
        await _next(context);

        // Nếu phản hồi là 401 Unauthorized, chỉnh sửa phản hồi
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.ContentType = "application/json";

            var response = new StandardResponse
            {
                Success = false,
                Message = "Unauthorized access",
                ExceptionMessage = "You are not authorized to access this resource."
            };

            var jsonResponse = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
