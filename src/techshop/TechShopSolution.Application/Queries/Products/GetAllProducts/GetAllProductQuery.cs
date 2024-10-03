using MediatR;
using TechShopSolution.Application.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts;

public class GetAllProductQuery : IRequest<IEnumerable<ProductDTO>?>
{
}