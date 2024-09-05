using MediatR;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoryQuery : IRequest<IEnumerable<CategoryDTO>?>
{
}