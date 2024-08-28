using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Application.Models.Categories;
using AutoMapper;

namespace TechShopSolution.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<CategoryDTO?> GetByIdAsync(int id)
    {
        var cate = await _categoryRepository.GetByIdAsync(id);

        var categoryDTO = _mapper.Map<CategoryDTO>(cate);

        return categoryDTO;
    }
}