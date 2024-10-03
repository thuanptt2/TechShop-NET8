using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Retrieve configuration settings
var connectionString = builder.Configuration.GetConnectionString("TechShopDB");
var kafkaBootstrapServers = builder.Configuration.GetSection("kafkaLoggingConfig:bootstrapServers").Value;
var healthChecksUIConfig = builder.Configuration.GetSection("HealthChecksUI");

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("sql_server", new SqlServerHealthCheck(connectionString), tags: new[] { "sql_server" })
    .AddCheck("kafka", new KafkaHealthCheck(kafkaBootstrapServers), tags: new[] { "kafka" }, timeout: TimeSpan.FromSeconds(5));

// Configure HealthChecks UI with SQLite Storage
builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(10); // Evaluation interval
})
.AddSqliteStorage(healthChecksUIConfig["ConnectionString"]);

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Add Health Check Endpoint
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Add HealthChecks UI Endpoint
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Path to access the Health Checks UI
    options.ApiPath = "/healthchecks-ui-api"; // API path for the UI
});

// Configure Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
