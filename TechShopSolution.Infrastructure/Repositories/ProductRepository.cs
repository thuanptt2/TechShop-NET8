using Microsoft.EntityFrameworkCore;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Infrastructure.DBContext;

namespace TechShopSolution.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly TechShopDbContext _context;

    public ProductRepository(TechShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
        .Include(b => b.Brand)
        .FirstOrDefaultAsync( x => x.Id == id);
    }
}