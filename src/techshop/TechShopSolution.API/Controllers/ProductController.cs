using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Application.Queries.Products.GetAllProducts;
using TechShopSolution.Application.Queries.Products.GetProductById;
using TechShopSolution.Application.Commands.Products.CreateProduct;
using TechShopSolution.Application.Commands.Products.DeleteProduct;
using TechShopSolution.Application.Commands.Products.UpdateProduct;
using TechShopSolution.Domain.Models.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TechShopSolution.Application.Queries.Products.GetProductsWithDynamicFilter;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        /// GET: api/v1/Product/GetAll
        [HttpGet("GetAll")]
        [Authorize(Policy = "JwtOrApiKey")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllV1()
        {
             var response = await mediator.Send(new GetAllProductQuery());

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        /// GET: api/v2/Product/GetAll
        [HttpGet("GetAll")]
        [Authorize(Policy = "JwtOrApiKey")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAllV2(int pageNumber = 1, int pageSize = 10)
        {
            var response = await mediator.Send(new GetAllProductV2Query {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = "apiKey")]
        public async Task<IActionResult> GetByIdV1(int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));

            if (product == null) 
            {
                return NotFound();
            }

            return Ok(product);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id:int}")]
        //[Authorize(AuthenticationSchemes = "apiKey")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdV2(int id)
        {
            var response = await mediator.Send(new GetProductByIdQuery(id));

            if (!response.Success) 
            {
                return response.Data == null ? NotFound(response) : BadRequest(response);
            }

            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsWithFilter(string filterExpression)
        {
            var query = new GetProductsWithDynamicFilterQuery(filterExpression);
            var result = await mediator.Send(query);

            if (!result.Success) 
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await mediator.Send(new DeleteProductCommand(id));

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }
        
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {         
            var response = await mediator.Send(command);

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            var response = await mediator.Send(command);

            if (!response.Success) 
            {
                BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult CheckHealth()
        {
            return Ok(new { status = "Healthy", message = "Product Service is healthy." });
        }
    }
}
