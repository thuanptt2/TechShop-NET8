using MediatR;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetProductById
{
    public class GetProductByIdQuery : IRequest<StandardResponse>
    {
        public int Id { get; set; }
        
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}