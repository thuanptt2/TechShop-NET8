using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Queries.Categories.GetCategoriesById;

public class GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger,
    IMapper mapper,
    IMemoryCacheService memoryCacheService,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, StandardResponse>
{
    public async Task<StandardResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting category {request.Id}");

        string cacheKey = $"Category:{request.Id}";
        var cachedCategory = await memoryCacheService.GetAsync<CategoryDTO>(cacheKey);
        if (cachedCategory != null)
        {
            return new StandardResponse
            {
                Success = true,
                Data = cachedCategory,
                Message = "Category retrieved from cache"
            };
        }

        var category = await categoryRepository.GetByIdAsync(request.Id);
        var cateDTO = mapper.Map<CategoryDTO?>(category);

        if(category != null)
        {            
            await memoryCacheService.SetAsync(cacheKey, cateDTO, TimeSpan.FromHours(1));
            return new StandardResponse
            {
                Success = true,
                Data = cateDTO,
                Message = "Category retrieved successfully"
            };
        }
        else
        {
            return new StandardResponse
            {
                Success = false,
                Message = $"Category with ID {request.Id} was not found"
            };
        }
    }
}