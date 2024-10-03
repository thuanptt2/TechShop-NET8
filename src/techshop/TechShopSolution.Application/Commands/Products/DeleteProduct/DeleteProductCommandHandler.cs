using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Commands.Products.DeleteProduct;

public class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger,
    IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting a product with id: " + request.Id);

        var product = await productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            logger.LogWarning("Product with id: " + request.Id + " not found");
            return false;
        }

        await productRepository.Delete(product);
        return true;
    }
}