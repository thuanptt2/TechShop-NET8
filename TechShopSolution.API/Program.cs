using TechShopSolution.Application.Extensions;
using TechShopSolution.Infrastructure.Extensions;
using TechShopSolution.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TechShopSolution.Infrastructure.Middlewares;
using TechShopSolution.Domain.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Suppress the default model validation response (use custom validation handling)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Enable directory browsing and static files serving
builder.Services.AddDirectoryBrowser();

// Configure OpenTelemetry
builder.Services.ConfigureOpentelemetry(builder.Configuration.GetValue<string>("otlpUrl"));

// Configure Serilog
builder.Services.ConfigureSerilog(
    builder.Configuration.GetSection(nameof(KafkaLoggingConfig)).Get<KafkaLoggingConfig>(),
    builder.Configuration.GetValue<string>("otlpUrl")
);

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // JWT Bearer
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    // API Key
    c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Description = "API Key needed to access the endpoints."
    });

    // Thêm yêu cầu bảo mật
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth" 
                }
            },
            new string[] {}
        },
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "apiKey" 
                }
            },
            new string[] {}
        }
    });
});
        
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build(); // Build the app here (only once)

// Ensure the logs directory exists
var logsPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "logs");
if (!Directory.Exists(logsPath))
{
    Directory.CreateDirectory(logsPath); // Create the logs directory if it doesn't exist
}

// Serve static files
app.UseStaticFiles();

// Enable directory browsing for logs
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(logsPath),
    RequestPath = "/logs"
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/identity").MapIdentityApi<User>();

// Register the custom middleware
app.UseMiddleware<CustomUnauthorizedMiddleware>();

app.UseAuthorization();

app.MapControllers();

// Add Health Check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
})
.WithMetadata(new AllowAnonymousAttribute());

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Path to access the Health Checks UI
});

app.Run();
