using AppCore.Interfaces;

namespace AppCore.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public string BuyerId { get; private set; }
        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; private set; }
        private List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal Total()
        {
            var total = 0m;
            foreach (var item in _items)
                total += item.UnitPrice * item.Units;
            return total;
        }
    }
}
