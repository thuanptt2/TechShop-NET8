using MediatR;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetProductsWithDynamicFilter
{
     public class GetProductsWithDynamicFilterQuery : IRequest<StandardResponse>
    {
        public string FilterExpression { get; }

        public GetProductsWithDynamicFilterQuery(string filterExpression)
        {
            FilterExpression = filterExpression;
        }
    }
}