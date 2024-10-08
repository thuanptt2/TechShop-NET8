using AutoMapper;
using MediatR;
using TechShopSolution.Application.Events;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Commands.Products.CreateProduct;

public class CreateProductCommandHandler(IMapper mapper,
    IProductRepository productRepository,
    IKafkaProducerService kafkaProducerService) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        var product = mapper.Map<Product>(request);

        product.CreateAt = DateTime.Now;

        int id = await productRepository.Create(product);

        // Tạo và gửi sự kiện lên Kafka
        var productCreatedEvent = mapper.Map<ProductCreatedEvent>(product);

        await kafkaProducerService.ProduceAsync("product-events", productCreatedEvent);

        return id;
    }
}