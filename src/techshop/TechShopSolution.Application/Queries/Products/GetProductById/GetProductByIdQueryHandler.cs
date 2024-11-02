using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Queries.Products.GetProductById;

public class GetProductByIdHandler(ILogger<GetProductByIdHandler> logger,
    IMapper mapper,
    IProductRepository productRepository,
    IRedisCacheService redisCacheService) : IRequestHandler<GetProductByIdQuery, StandardResponse>
{
    public async Task<StandardResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Product:{request.Id}";
        
        // Kiểm tra trong cache
        var cachedProduct = await redisCacheService.GetCacheAsync<ProductDTO>(cacheKey);
        if (cachedProduct != null)
        {
            return new StandardResponse
            {
                Success = true,
                Data = cachedProduct,
                Message = "Product retrieved from cache"
            };
        }

        // Lấy sản phẩm từ cơ sở dữ liệu
        var product = await productRepository.GetByIdAsync(request.Id);
        var productDTO = mapper.Map<ProductDTO?>(product);

        if (productDTO != null)
        {
            await redisCacheService.SetCacheAsync(cacheKey, productDTO, TimeSpan.FromHours(1));
            return new StandardResponse
            {
                Success = true,
                Data = productDTO,
                Message = "Product retrieved successfully"
            };
        }
        else
        {
            return new StandardResponse
            {
                Success = false,
                Message = $"Product with ID {request.Id} was not found"
            };
        }
    }
}