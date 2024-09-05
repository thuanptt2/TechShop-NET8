using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Application.Queries.Products.GetAllProducts;
using TechShopSolution.Application.Queries.Products.GetProductById;
using TechShopSolution.Application.Commands.Products.CreateProduct;
using TechShopSolution.Application.Commands.Products.DeleteProduct;
using TechShopSolution.Application.Commands.Products.UpdateProduct;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll() {
            var products = await mediator.Send(new GetAllProductQuery());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO?>> GetById(int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpGet("DeleteProduct/{id}")]
        public async Task<ActionResult<ProductDTO?>> DeleteProduct(int id)
        {
            var isDeleted = await mediator.Send(new DeleteProductCommand(id));
            if (isDeleted) return NoContent();

            return NotFound();
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            try {
                if(!ModelState.IsValid) return BadRequest(ModelState);
            
                var id = await mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id }, null);
            }
            catch(Exception ex)
            {
                //logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            try {
                if(!ModelState.IsValid) return BadRequest(ModelState);
            
                var id = await mediator.Send(command);
                return NoContent();
            }
            catch(Exception ex)
            {
                //logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
