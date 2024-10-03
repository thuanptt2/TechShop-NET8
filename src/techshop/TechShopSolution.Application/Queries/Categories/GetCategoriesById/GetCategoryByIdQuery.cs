using MediatR;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Queries.Categories.GetCategoriesById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDTO?>
    {
        public int Id { get; set; }
        
        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}