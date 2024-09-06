using TechShopSolution.Application.Extensions;
using TechShopSolution.Infrastructure.Extensions;
using TechShopSolution.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOpentelemetry(builder.Configuration.GetValue<string>("otlpUrl"));
builder.Services.ConfigureSerilog(builder.Configuration.GetSection(nameof(KafkaLoggingConfig)).Get<KafkaLoggingConfig>(),
    builder.Configuration.GetValue<string>("otlpUrl"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
