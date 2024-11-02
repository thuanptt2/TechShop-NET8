using MediatR;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Common;

namespace TechShopSolution.Application.Queries.Categories.GetCategoriesById
{
    public class GetCategoryByIdQuery : IRequest<StandardResponse>
    {
        public int Id { get; set; }
        
        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}