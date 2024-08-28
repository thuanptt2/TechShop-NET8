using Microsoft.EntityFrameworkCore;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Infrastructure.DBContext;

namespace TechShopSolution.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TechShopDbContext _context;

    public CategoryRepository(TechShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
        .Include(p => p.ProductInCategory)
        .ThenInclude(pc => pc.Product) 
        .ThenInclude(b => b.Brand)
        .FirstOrDefaultAsync( x => x.Id == id);
    }
}