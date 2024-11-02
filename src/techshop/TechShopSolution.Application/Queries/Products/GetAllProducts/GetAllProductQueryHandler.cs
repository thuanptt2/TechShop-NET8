using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts;

public class GetAllProductQueryHandler(ILogger<GetAllProductQueryHandler> logger,
    IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<GetAllProductQuery, StandardResponse>
{
    public async Task<StandardResponse> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all products");
        var products = await productRepository.GetAllAsync();
        var productDTO = mapper.Map<IEnumerable<ProductDTO>?>(products);

        return new StandardResponse
        {
            Success = true,
            Data = productDTO,
            Message = "Product retrieved successfully"
        };
    }
}