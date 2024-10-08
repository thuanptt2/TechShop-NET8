using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoryQueryHandler(ILogger<GetAllCategoryQueryHandler> logger,
    IMapper mapper,
    ICategoryRepository categoryRepository) : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryDTO>?>
{
    public async Task<IEnumerable<CategoryDTO>?> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all categories");
        var categories = await categoryRepository.GetAllAsync();
        var cateDTO = mapper.Map<IEnumerable<CategoryDTO>?>(categories);
        return cateDTO;
    }
}