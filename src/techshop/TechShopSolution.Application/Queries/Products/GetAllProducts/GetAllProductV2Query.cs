using MediatR;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts;

public class GetAllProductV2Query : IRequest<StandardResponse>
{
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
}