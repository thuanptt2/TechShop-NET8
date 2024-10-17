using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Events;
using TechShopSolution.Core.Event;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Infrastructure.EventHandlers
{
    public class ProductDeletedEventHandler : IEventHandler<ProductDeletedEvent>
    {
        private readonly IProductMongoRepository _productmongoRepository;
        private readonly ILogger<ProductDeletedEventHandler> _logger;

        public ProductDeletedEventHandler(IProductMongoRepository productmongoRepository, ILogger<ProductDeletedEventHandler> logger)
        {
            _productmongoRepository = productmongoRepository;
            _logger = logger;
        }

        public async Task Handle(ProductDeletedEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _productmongoRepository.GetByIdAsync(@event.Id);
                if (existingProduct != null)
                {
                    await _productmongoRepository.DeleteAsync(@event.Id);
                    _logger.LogInformation($"Product with ID {@event.Id} has been deleted from MongoDB.");
                }
                else
                {
                    _logger.LogWarning($"Product with ID {@event.Id} not found in MongoDB.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {@event.Id}: {ex.Message}");
            }
        }
    }
}
