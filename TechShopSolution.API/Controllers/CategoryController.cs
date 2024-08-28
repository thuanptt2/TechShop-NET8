using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechShopSolution.Application.Services.Categories;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cate = await categoryService.GetByIdAsync(id);
            if (cate == null) return NotFound();

            return Ok(cate);
        }
    }
}
