using TechShopSolution.Application.Models.Products;
using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Application.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<int> Create(CreateProductDTO dto);
    }
}