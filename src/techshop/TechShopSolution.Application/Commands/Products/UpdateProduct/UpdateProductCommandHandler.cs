using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Events;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Commands.Products.UpdateProduct;

public class UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger,
    IMapper mapper,
    IKafkaProducerService kafkaProducerService,
    IProductRepository productRepository,
    IRedisCacheService redisCacheService) : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a product with id: " + request.Id);

        var product = await productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            logger.LogWarning("Product with id: " + request.Id + " not found");
            return false;
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
        
        return true;
    }
}