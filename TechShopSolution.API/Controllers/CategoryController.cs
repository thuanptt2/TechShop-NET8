using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechShopSolution.Application.Queries.Categories.GetAllCategories;
using TechShopSolution.Application.Queries.Categories.GetCategoriesById;
using TechShopSolution.Application.Models.Common;
using TechShopSolution.Application.Models.Categories;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController(IMediator mediator, ILogger<ProductController> logger) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = new StandardResponse<IEnumerable<CategoryDTO>>();
            
            try 
            {
                var categories = await mediator.Send(new GetAllCategoryQuery());

                response.Success = true;
                response.Data = categories;
                response.Message = "Categories retrieved successfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(JsonConvert.SerializeObject(ex));

                response.Success = false;
                response.Message = "An unexpected error occurred";
                response.ExceptionMessage = ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = new StandardResponse<CategoryDTO>();
            
            try 
            {
                var category = await mediator.Send(new GetCategoryByIdQuery(id));

                if (category == null)
                {
                    response.Success = false;
                    response.Message = $"Category with ID {id} not found";
                    return NotFound(response);
                }

                response.Success = true;
                response.Data = category;
                response.Message = "Category retrieved successfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(JsonConvert.SerializeObject(ex));

                response.Success = false;
                response.Message = "An unexpected error occurred";
                response.ExceptionMessage = ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
