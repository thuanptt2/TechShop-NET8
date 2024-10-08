using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Queries.Products.GetProductById;

public class GetProductByIdHandler(ILogger<GetProductByIdHandler> logger,
    IMapper mapper,
    IProductRepository productRepository,
    IRedisCacheService redisCacheService) : IRequestHandler<GetProductByIdQuery, ProductDTO?>
{
    public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting product {request.Id}");

        string cacheKey = $"Product:{request.Id}";
        var cachedProduct = await redisCacheService.GetCacheAsync<ProductDTO>(cacheKey);
        if (cachedProduct != null)
        {
            return cachedProduct;
        }

        var product = await productRepository.GetByIdAsync(request.Id);
        
        var productDTO = mapper.Map<ProductDTO?>(product);

        if(product != null)
        {            
            await redisCacheService.SetCacheAsync(cacheKey, productDTO, TimeSpan.FromHours(1));
        }

        return productDTO;
    }
}