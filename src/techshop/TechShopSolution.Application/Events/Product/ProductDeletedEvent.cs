using TechShopSolution.Application.Event;

namespace TechShopSolution.Application.Events
{
    public class ProductDeletedEvent : BaseEvent
    {
        public ProductDeletedEvent()
        {
            EventType = nameof(ProductDeletedEvent);
        }

        public int Id { get; set; }
    }
}
