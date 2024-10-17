using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Core.Event;
using TechShopSolution.Domain.Models;
using AutoMapper;
using TechShopSolution.Application.Events;

namespace TechShopSolution.Infrastructure.EventHandlers
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreatedEvent>
    {
        private readonly IProductMongoRepository _productMongoRepository;
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IMapper _mapper;

        public ProductCreatedEventHandler(IProductMongoRepository productMongoRepository, ILogger<ProductCreatedEventHandler> logger, IMapper mapper)
        {
            _productMongoRepository = productMongoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(ProductCreatedEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _productMongoRepository.GetByIdAsync(@event.Id);
                if (existingProduct == null)
                {
                    var product = _mapper.Map<MongoProduct>(@event);

                    await _productMongoRepository.CreateAsync(product);
                    
                    _logger.LogInformation($"Product with ID {@event.Id} has been created in MongoDB.");
                }
                else
                {
                    _logger.LogInformation($"Product with ID {@event.Id} already exists in MongoDB.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating product in MongoDB: {ex.Message}");
            }
        }
    }
}
