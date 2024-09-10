using TechShopSolution.Application.Extensions;
using TechShopSolution.Infrastructure.Extensions;
using TechShopSolution.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.FileProviders;
using TechShopSolution.Infrastructure.Middlewares;

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

// Set up Serilog for logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .MinimumLevel.Debug()
    .WriteTo.Console() // Log to console for all levels
    .WriteTo.File("wwwroot/logs/error/error.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Error logs
    .WriteTo.File("wwwroot/logs/info/info.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Info logs
    .WriteTo.File("wwwroot/logs/trace/traces.txt", LogEventLevel.Debug, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Trace logs
    .ReadFrom.Configuration(context.Configuration));

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();

// Register the custom middleware
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
