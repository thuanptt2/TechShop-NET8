using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Application.Queries.Categories.GetAllCategories;
using TechShopSolution.Application.Queries.Categories.GetCategoriesById;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() {
            var categories = await mediator.Send(new GetAllCategoryQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cate = await mediator.Send(new GetCategoryByIdQuery(id));
            if (cate == null) return NotFound();

            return Ok(cate);
        }
    }
}
