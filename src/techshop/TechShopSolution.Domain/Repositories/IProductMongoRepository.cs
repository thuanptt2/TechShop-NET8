using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models;

namespace TechShopSolution.Domain.Repositories
{
    public interface IProductMongoRepository
    {
        Task<MongoProduct?> GetByIdAsync(int id);
        Task<int> CreateAsync(MongoProduct product);
        Task DeleteAsync(int id);
        Task UpdateAsync(MongoProduct product);
    }
}
