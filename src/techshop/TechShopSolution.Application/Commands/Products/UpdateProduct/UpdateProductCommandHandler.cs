using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Events;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Commands.Products.UpdateProduct;

public class UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger,
    IMapper mapper,
    IKafkaProducerService kafkaProducerService,
    IProductRepository productRepository,
    IRedisCacheService redisCacheService,
    UpdateProductCommandValidator validator) : IRequestHandler<UpdateProductCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

            return new StandardResponse
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            };
        }
        
        logger.LogInformation("Updating a product with id: " + request.Id);

        var product = await productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            return new StandardResponse
            {
                Success = false,
                Message = $"Product with ID {request.Id} was not found"
            };
        }

        mapper.Map(request, product);

        product.UpdateAt = DateTime.Now;

        await productRepository.SaveChanges();

        // Tạo và gửi sự kiện lên Kafka
        var productUpdatedEvent = mapper.Map<ProductUpdatedEvent>(product);
        await kafkaProducerService.ProduceAsync("product-events", productUpdatedEvent);

        // Cập nhật cache
        string cacheKey = $"Product:{request.Id}";
        var productDTO = mapper.Map<ProductDTO>(product);
        await redisCacheService.SetCacheAsync(cacheKey, productDTO, TimeSpan.FromHours(1));
        
        return new StandardResponse
        {
            Success = true,
            Message = $"Update product with ID {request.Id} successfully",
        };
    }
}