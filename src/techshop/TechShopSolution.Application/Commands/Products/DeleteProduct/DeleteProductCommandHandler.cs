using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Events;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Commands.Products.DeleteProduct;

public class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger,
    IKafkaProducerService kafkaProducerService,
    IProductRepository productRepository,
    IRedisCacheService redisCacheService) : IRequestHandler<DeleteProductCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting a product with id: " + request.Id);

        var product = await productRepository.GetByIdAsync(request.Id);

        // Tạo và gửi sự kiện lên Kafka
        var productDeletedEvent = new ProductDeletedEvent { Id = request.Id };
        await kafkaProducerService.ProduceAsync("product-events", productDeletedEvent);

        if (product == null)
        {
            return new StandardResponse
            {
                Success = false,
                Message = $"Product with ID {request.Id} was not found"
            };
        }

        await productRepository.Delete(product);

        // Cập nhật cache
        string cacheKey = $"Product:{request.Id}";
        await redisCacheService.RemoveCacheAsync(cacheKey);

        return new StandardResponse
        {
            Success = true,
            Message = $"Delete product with ID {request.Id} successfully",
        };
    }
}