using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Application.Queries.Products;

namespace TechShopSolution.Application.Queries.Products.GetProductById;

public class GetProductByIdHandler(ILogger<GetProductByIdHandler> logger,
    IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, ProductDTO?>
{
    public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting product {request.Id}");
        var product = await productRepository.GetByIdAsync(request.Id);
        var productDTO = mapper.Map<ProductDTO?>(product);

        return productDTO;
    }
}