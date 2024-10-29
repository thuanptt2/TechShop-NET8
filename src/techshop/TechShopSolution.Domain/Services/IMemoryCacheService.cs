
namespace TechShopSolution.Domain.Services
{
    public interface IMemoryCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan cacheDuration);
        void Remove(string key);
    }
}
