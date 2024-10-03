using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;
using TechShopSolution.Infrastructure.Auth;
using TechShopSolution.Infrastructure.DBContext;
using TechShopSolution.Infrastructure.Repositories;
using TechShopSolution.Infrastructure.Services;

namespace TechShopSolution.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TechShopDB");
        var kafkaBootstrapServers = configuration.GetSection("kafkaLoggingConfig:bootstrapServers").Value;

        services.AddDbContext<TechShopDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<TechShopDbContext>();
        
        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("apiKey", options => { });

        // Cấu hình Authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy("JwtOrApiKey", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "apiKey");
                policy.RequireAuthenticatedUser();
            });
        });
        
        var healthChecksUI = configuration.GetSection("HealthChecksUI");
        services.AddHealthChecks()
            .AddCheck("sql_server", new SqlServerHealthCheck(connectionString), tags: new[] { "sql_server" })
            .AddCheck("kafka", new KafkaHealthCheck(kafkaBootstrapServers), tags: new[] { "kafka" }, timeout: TimeSpan.FromSeconds(5));

        services.AddHealthChecksUI()
            .AddSqliteStorage(healthChecksUI["ConnectionString"]);

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}