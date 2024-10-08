using MediatR;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts;

public class GetAllProductV2Query : IRequest<(IEnumerable<ProductDTO>?, int)>
{
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
}