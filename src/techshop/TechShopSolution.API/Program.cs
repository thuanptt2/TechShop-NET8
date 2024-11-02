using TechShopSolution.Application.Extensions;
using TechShopSolution.Infrastructure.Extensions;
using TechShopSolution.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TechShopSolution.Infrastructure.Middlewares;
using TechShopSolution.Domain.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Suppress the default model validation response (use custom validation handling)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Configure KafkaLoggingConfig
builder.Services.Configure<KafkaLoggingConfig>(builder.Configuration.GetSection("kafkaLoggingConfig"));

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

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60, // Maximum number of requests allowed
                Window = TimeSpan.FromMinutes(1), // Time window (e.g., 1 minute)
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0 // Number of requests allowed to be queued
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);
    };
});
        

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

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
        
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build(); // Build the app here (only once)

app.UseHttpLogging();

// Ensure the logs directory exists
var logsPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "logs");
if (!Directory.Exists(logsPath))
{
    Directory.CreateDirectory(logsPath); // Create the logs directory if it doesn't exist
}

// Serve static files
app.UseStaticFiles();

app.UseRateLimiter();

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

app.MapGroup("api/identity")
    .MapIdentityApi<User>();

// Register the custom middleware
app.UseMiddleware<CustomUnauthorizedMiddleware>();
//app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();