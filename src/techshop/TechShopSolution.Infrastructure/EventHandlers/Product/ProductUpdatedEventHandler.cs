
using AutoMapper;
using Microsoft.Extensions.Logging;
using TechShopSolution.Application.Events;
using TechShopSolution.Core.Event;
using TechShopSolution.Domain.Models;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Infrastructure.EventHandlers
{
    public class ProductUpdatedEventHandler : IEventHandler<ProductUpdatedEvent>
    {
        private readonly IProductMongoRepository _productMongoRepository;
        private readonly ILogger<ProductUpdatedEventHandler> _logger;
        private readonly IMapper _mapper;

        public ProductUpdatedEventHandler(IProductMongoRepository productMongoRepository, ILogger<ProductUpdatedEventHandler> logger, IMapper mapper)
        {
            _productMongoRepository = productMongoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(ProductUpdatedEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _productMongoRepository.GetByIdAsync(@event.Id);
                if (existingProduct != null)
                {
                   var product = _mapper.Map<MongoProduct>(@event);

                    await _productMongoRepository.UpdateAsync(product);
                    _logger.LogInformation($"Product with ID {@event.Id} has been updated in MongoDB.");
                }
                else
                {
                    _logger.LogWarning($"Product with ID {@event.Id} not found in MongoDB.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {@event.Id}: {ex.Message}");
            }
        }
    }
}
