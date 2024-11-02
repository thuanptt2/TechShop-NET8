using MediatR;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Common;

namespace TechShopSolution.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoryQuery : IRequest<StandardResponse>
{
}