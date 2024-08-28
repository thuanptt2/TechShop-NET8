using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechShopSolution.Application.Services.Products;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
    }
}
