using TechShopSolution.Domain.Entities;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Services.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
    }
}