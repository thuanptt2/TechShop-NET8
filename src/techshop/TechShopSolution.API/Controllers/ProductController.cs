using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Application.Queries.Products.GetAllProducts;
using TechShopSolution.Application.Queries.Products.GetProductById;
using TechShopSolution.Application.Commands.Products.CreateProduct;
using TechShopSolution.Application.Commands.Products.DeleteProduct;
using TechShopSolution.Application.Commands.Products.UpdateProduct;
using TechShopSolution.Application.Models.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TechShopSolution.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductController(IMediator mediator, 
        ILogger<ProductController> logger) : ControllerBase
    {
        /// GET: api/v1/Product/GetAll
        [HttpGet("GetAll")]
        [Authorize(Policy = "JwtOrApiKey")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllV1()
        {
            var response = new StandardResponse<IEnumerable<ProductDTO>>();

            try
            {
                var products = await mediator.Send(new GetAllProductQuery());

                response.Success = true;
                response.Data = products;
                response.Message = "Products retrieved successfully - V1";

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

        /// GET: api/v2/Product/GetAll
        [HttpGet("GetAll")]
        [Authorize(Policy = "JwtOrApiKey")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAllV2(int pageNumber = 1, int pageSize = 10)
        {
            var response = new StandardResponse<IEnumerable<ProductDTO>>();

            try
            {
                var (products, totalRecords) = await mediator.Send(new GetAllProductV2Query
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

                response.Success = true;
                response.Data = products;
                response.Message = "Products retrieved successfully - V2";
                response.Paging = new Paging
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                };

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

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "apiKey")]
        public async Task<IActionResult> GetByIdV1(int id)
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));

            if (product == null) 
            {
                return NotFound();
            }

            decimal largeDecimalValue = 3.0e10M;
            int intValue = (int)largeDecimalValue;

            return Ok(product);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "apiKey")]
        public async Task<IActionResult> GetByIdV2(int id)
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

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize]
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
        [Authorize]
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
