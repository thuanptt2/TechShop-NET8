using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<int> Create(Product product);
    Task Delete(Product product);
    Task SaveChanges();
}