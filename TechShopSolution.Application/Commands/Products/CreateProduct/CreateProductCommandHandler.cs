using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Commands.Products.CreateProduct;

public class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger,
    IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new product");

        var product = mapper.Map<Product>(request);

        int id = await productRepository.Create(product);
        return id;
    }
}