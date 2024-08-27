using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Application.Products
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
    }
}