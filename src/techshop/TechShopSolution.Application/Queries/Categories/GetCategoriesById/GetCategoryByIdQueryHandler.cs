using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Models.Categories;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Application.Queries.Categories;

namespace TechShopSolution.Application.Queries.Categories.GetCategoriesById;

public class GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger,
    IMapper mapper,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting category {request.Id}");
        var category = await categoryRepository.GetByIdAsync(request.Id);
        var cateDTO = mapper.Map<CategoryDTO?>(category);

        return cateDTO;
    }
}