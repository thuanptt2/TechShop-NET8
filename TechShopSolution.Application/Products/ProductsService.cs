using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Products;

public class ProductsService(IProductsRepository productsRepository) : IProductsService
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await productsRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await productsRepository.GetByIdAsync(id);
    }
}