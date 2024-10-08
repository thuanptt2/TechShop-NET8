using System;

namespace TechShopSolution.Application.Event
{
    public class BaseEvent : IEvent
    {
        public string EventType { get; protected set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
