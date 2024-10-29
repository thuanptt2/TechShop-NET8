using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Infrastructure.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T cacheValue))
            {
                return Task.FromResult<T?>(cacheValue);
            }
            
            return Task.FromResult<T?>(default);
        }
        public Task SetAsync<T>(string key, T value, TimeSpan cacheDuration)
        {
            _memoryCache.Set(key, value, cacheDuration);
            return Task.CompletedTask;
        }
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
