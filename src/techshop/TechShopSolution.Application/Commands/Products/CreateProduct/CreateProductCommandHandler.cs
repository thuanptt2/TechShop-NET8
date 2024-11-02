using AutoMapper;
using FluentValidation.Results;
using MediatR;
using TechShopSolution.Application.Events;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Application.Commands.Products.CreateProduct;

public class CreateProductCommandHandler(IMapper mapper,
    IProductRepository productRepository,
    IKafkaProducerService kafkaProducerService,
    CreateProductCommandValidator validator) : IRequestHandler<CreateProductCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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
        
        var product = mapper.Map<Product>(request);
        product.CreateAt = DateTime.Now;

        int id = await productRepository.Create(product);

        // Tạo và gửi sự kiện lên Kafka
        var productCreatedEvent = mapper.Map<ProductCreatedEvent>(product);
        await kafkaProducerService.ProduceAsync("product-events", productCreatedEvent);

        var response = new StandardResponse
        {
            Success = true,
            Message = "Product created successfully",
            Data = id
        };

        return response;
    }
}