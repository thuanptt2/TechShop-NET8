using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using AutoMapper;
using TechShopSolution.Application.Models.Products;

namespace TechShopSolution.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<ProductDTO?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        var productDTO = _mapper.Map<ProductDTO>(product);

        return productDTO;
    }
}