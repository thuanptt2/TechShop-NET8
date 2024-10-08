using MongoDB.Driver;
using TechShopSolution.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using TechShopSolution.Domain.Models;
using Microsoft.Extensions.Logging;

namespace TechShopSolution.Infrastructure.Repositories
{
    public class ProductMongoRepository : IProductMongoRepository
    {
        private readonly IMongoCollection<MongoProduct> _productsCollection;
        private readonly AsyncPolicy _policy;
        private readonly ILogger<ProductMongoRepository> _logger;

        public ProductMongoRepository(IMongoClient mongoClient, IConfiguration configuration, ILogger<ProductMongoRepository> logger)
        {
            var mongoDatabase = mongoClient.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _productsCollection = mongoDatabase.GetCollection<MongoProduct>("Products");

            _logger = logger;

            var retryPolicy = Policy
                .Handle<MongoException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} due to {exception.Message}");
                    }
                );

            var circuitBreakerPolicy = Policy
                .Handle<MongoException>()
                .Or<TaskCanceledException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, breakDelay) =>
                    {
                       _logger.LogWarning($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to {exception.Message}");
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker reset.");
                    }
                );

            // Kết hợp Retry với Circuit Breaker
            var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(30));
            _policy = Policy.WrapAsync(timeoutPolicy, retryPolicy, circuitBreakerPolicy);
        }

        public async Task<MongoProduct?> GetByIdAsync(int id)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                return await _productsCollection
                    .Find(p => p.Id == id)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> CreateAsync(MongoProduct product)
        {
            await _policy.ExecuteAsync(async () =>
            {
                await _productsCollection.InsertOneAsync(product);
            });

            return product.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await _policy.ExecuteAsync(async () =>
            {
                var deleteResult = await _productsCollection.DeleteOneAsync(p => p.Id == id);
            });
        }

        public async Task UpdateAsync(MongoProduct product)
        {
            await _policy.ExecuteAsync(async () =>
            {
                await _productsCollection.ReplaceOneAsync(p => p.Id == product.Id, product);
            });
        }
    }
}
