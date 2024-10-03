using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}