using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Commands.Products.UpdateProduct;

public class UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger,
    IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
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

        await productRepository.SaveChanges();
        return true;
    }
}