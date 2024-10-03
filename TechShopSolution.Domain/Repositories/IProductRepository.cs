using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<int> Create(Product product);
    Task Delete(Product product);
    Task SaveChanges();
}