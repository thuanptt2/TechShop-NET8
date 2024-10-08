using MediatR;
using TechShopSolution.Domain.Models.Categories;

namespace TechShopSolution.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoryQuery : IRequest<IEnumerable<CategoryDTO>?>
{
}