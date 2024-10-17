using AutoMapper;
using MediatR;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Queries.Products.GetProductsWithDynamicFilter;

public class GetProductsWithDynamicFilterQueryHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<GetProductsWithDynamicFilterQuery, IEnumerable<ProductDTO>?>
{
    public async Task<IEnumerable<ProductDTO>?> Handle(GetProductsWithDynamicFilterQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductsWithDynamicFilter(request.FilterExpression);
        var productDTO = mapper.Map<IEnumerable<ProductDTO>?>(products);
        return productDTO;
    }
}