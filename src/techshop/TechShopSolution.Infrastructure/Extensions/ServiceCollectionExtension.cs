using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using TechShopSolution.Application.Events;
using TechShopSolution.Core.Event;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;
using TechShopSolution.Infrastructure.Auth;
using TechShopSolution.Infrastructure.DBContext;
using TechShopSolution.Infrastructure.Repositories;
using TechShopSolution.Infrastructure.Services;
using TechShopSolution.Infrastructure.EventHandlers;
using MongoDB.Driver;
using MediatR;
using MediatR.Pipeline;

namespace TechShopSolution.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("TechShopDB");
        var kafkaBootstrapServers = configuration.GetSection("kafkaLoggingConfig:bootstrapServers").Value;
        services.AddDbContext<TechShopDbContext>(options => options.UseSqlServer(connectionString));

        var mongoConnectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
        var mongoClient = new MongoClient(mongoConnectionString);
        var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);
        services.AddSingleton<IMongoClient>(mongoClient);

        // Cấu hình Connection Pool
        mongoClientSettings.MaxConnectionPoolSize = 100;
        
        // Cấu hình Timeout
        mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(20);
        mongoClientSettings.SocketTimeout = TimeSpan.FromSeconds(20);

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
    
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductMongoRepository, ProductMongoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<IVaultService, VaultService>();

        // Register Kafka Producer Service
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

        // Register Kafka Consumer as Hosted Service
        services.AddHostedService<KafkaConsumerService>();
        services.AddScoped<IEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
        services.AddScoped<IEventHandler<ProductDeletedEvent>, ProductDeletedEventHandler>();
        services.AddScoped<IEventHandler<ProductUpdatedEvent>, ProductUpdatedEventHandler>();

        // Register Redis Service
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var config= ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(config);
        });
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddMemoryCache();
        services.AddScoped<IMemoryCacheService, MemoryCacheService>();

        
    }
}