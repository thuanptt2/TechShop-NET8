using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoryQueryHandler(ILogger<GetAllCategoryQueryHandler> logger,
    IMapper mapper,
    ICategoryRepository categoryRepository) : IRequestHandler<GetAllCategoryQuery, StandardResponse>
{
    public async Task<StandardResponse> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all categories");
        var categories = await categoryRepository.GetAllAsync();
        var cateDTO = mapper.Map<IEnumerable<CategoryDTO>?>(categories);

        return new StandardResponse
        {
            Success = true,
            Data = cateDTO,
            Message = "Category retrieved successfully"
        };
    }
}