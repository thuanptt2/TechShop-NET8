using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechShopSolution.Application.Products;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IProductsService productsService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() {
            var products = await productsService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productsService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
    }
}
