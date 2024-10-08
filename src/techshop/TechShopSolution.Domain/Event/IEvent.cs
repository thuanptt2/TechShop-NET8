namespace TechShopSolution.Application.Event
{
    public interface IEvent
    {
        string EventType { get; }
        DateTime Timestamp { get; }
    }
}
