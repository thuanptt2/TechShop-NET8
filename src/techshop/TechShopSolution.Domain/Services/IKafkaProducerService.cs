using TechShopSolution.Application.Event;

namespace TechShopSolution.Domain.Services
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(string topic, IEvent @event);
    }
}
