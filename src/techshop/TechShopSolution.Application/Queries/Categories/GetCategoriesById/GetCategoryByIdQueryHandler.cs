using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Queries.Categories.GetCategoriesById;

public class GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger,
    IMapper mapper,
    IMemoryCacheService memoryCacheService,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting category {request.Id}");

        string cacheKey = $"Category:{request.Id}";
        var cachedCategory = await memoryCacheService.GetAsync<CategoryDTO>(cacheKey);
        if (cachedCategory != null)
        {
            return cachedCategory;
        }

        var category = await categoryRepository.GetByIdAsync(request.Id);
        var cateDTO = mapper.Map<CategoryDTO?>(category);

        if(category != null)
        {            
            await memoryCacheService.SetAsync(cacheKey, cateDTO, TimeSpan.FromHours(1));
        }

        return cateDTO;
    }
}