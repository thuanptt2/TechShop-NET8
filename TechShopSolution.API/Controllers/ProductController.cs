﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Application.Queries.Products.GetAllProducts;
using TechShopSolution.Application.Queries.Products.GetProductById;
using TechShopSolution.Application.Commands.Products.CreateProduct;
using TechShopSolution.Application.Commands.Products.DeleteProduct;
using TechShopSolution.Application.Commands.Products.UpdateProduct;
using Serilog;
using TechShopSolution.Application.Models.Common;
using Newtonsoft.Json;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IMediator mediator, ILogger<ProductController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll() 
        {
            var response = new StandardResponse<IEnumerable<ProductDTO>>();
            
            try 
            {
                var products = await mediator.Send(new GetAllProductQuery());

                response.Success = true;
                response.Data = products;
                response.Message = "Products retrieved successfully";

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
            var response = new StandardResponse<ProductDTO>();
            
            try 
            {
                var product = await mediator.Send(new GetProductByIdQuery(id));

                if (product == null) 
                {
                    response.Success = false;
                    response.Message = $"Product with ID {id} was not found";
                    return NotFound(response);
                }

                response.Success = true;
                response.Data = product;
                response.Message = "Product retrieved successfully";

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

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = new StandardResponse<object>();
            
            try 
            {
                var isDeleted = await mediator.Send(new DeleteProductCommand(id));

                if (!isDeleted)
                {
                    response.Success = false;
                    response.Message = $"Product with id: {id} not found";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Product deleted successfully";
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

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var response = new StandardResponse<object>();
            
            try 
            {
                if (!ModelState.IsValid) 
                {
                    response.Success = false;
                    response.Message = "Invalid request data";
                    response.ErrorData = ModelState;
                    return BadRequest(response);
                }
                
                var id = await mediator.Send(command);

                response.Success = true;
                response.Message = "Product created successfully";
                response.Data = new { ProductId = id };
                
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

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            var response = new StandardResponse<object>();
            
            try 
            {
                if (!ModelState.IsValid)
                {
                    response.Success = false;
                    response.Message = "Invalid request data";
                    response.ErrorData = ModelState;
                    return BadRequest(response);
                }

                var isUpdated = await mediator.Send(command);

                if (!isUpdated)
                {
                    response.Success = false;
                    response.Message = "Failed to update product";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Product updated successfully";
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
