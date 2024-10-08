using TechShopSolution.Application.Event;

namespace TechShopSolution.Core.Event
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
