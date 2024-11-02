using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechShopSolution.Application.Queries.Categories.GetAllCategories;
using TechShopSolution.Application.Queries.Categories.GetCategoriesById;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Categories;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "JwtOrApiKey")]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await mediator.Send(new GetAllCategoryQuery());

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await mediator.Send(new GetCategoryByIdQuery(id));

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }
    }
}
