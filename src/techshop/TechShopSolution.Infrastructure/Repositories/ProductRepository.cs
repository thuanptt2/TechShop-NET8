using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Infrastructure.DBContext;
using TechShopSolution.Infrastructure.Helper;
using System.Linq.Dynamic.Core;

namespace TechShopSolution.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly TechShopDbContext _context;
    private readonly IMapper _mapper;
    public ProductRepository(TechShopDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
        .Include(p => p.ProductInCategory!)
        .ThenInclude(pc => pc.Category!)
        .Include(b => b.Brand!).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Products
            .Include(p => p.ProductInCategory!)
                .ThenInclude(pc => pc.Category!)
            .Include(b => b.Brand!)
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize) 
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
        .Include(p => p.ProductInCategory!)
        .ThenInclude(pc => pc.Category!)
        .Include(b => b.Brand!)
        .FirstOrDefaultAsync( x => x.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsWithDynamicFilter(string filterExpression)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filterExpression))
        {
            query = query.Where(filterExpression);
        }

        return await query
            .Include(p => p.ProductInCategory!)
                .ThenInclude(pc => pc.Category!)
            .Include(p => p.Brand!)
            .ToListAsync();
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

        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs", "product.json");

        var productDTO = _mapper.Map<ProductDTO>(entity);

        await JsonFileHelper.AddItemToJsonFileAsync<ProductDTO>(jsonFilePath, productDTO);

        return entity.Id;
    }

    public async Task Delete(Product product)
    {
        _context.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}