using MediatR;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts;

public class GetAllProductQuery : IRequest<StandardResponse>
{
}