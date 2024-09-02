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
        .Include(p => p.ProductInCategory!)
        .ThenInclude(pc => pc.Category!)
        .Include(b => b.Brand!)
        .FirstOrDefaultAsync( x => x.Id == id);
    }

    public async Task<int> Create(Product entity)
    {
        _context.Products.Add(entity);

        if(entity.ProductInCategory?.Count > 0)
        {
            foreach (var item in entity.ProductInCategory)
            {
                item.ProductId = entity.Id;
            }

            _context.CategoryProducts.AddRange(entity.ProductInCategory);
        }
        await _context.SaveChangesAsync();
        
        return entity.Id;
    }
}