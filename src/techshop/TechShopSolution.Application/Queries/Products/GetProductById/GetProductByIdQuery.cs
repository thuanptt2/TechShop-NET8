using MediatR;
using TechShopSolution.Domain.Models.Products;

namespace TechShopSolution.Application.Queries.Products.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDTO?>
    {
        public int Id { get; set; }
        
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}