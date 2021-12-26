using SharedKernel.Interfaces;

namespace AppCore.Entities.BasketAggregate
{
    public class Basket : BaseEntity, IAggregateRoot
    {
        public string BuyerId { get; private set; }
        private readonly List<BasketItem> _items = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public Basket(string buyerId)
        {
            BuyerId = buyerId;
        }

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(e => e.CatalogItemId == catalogItemId))
            {
                _items.Add(new BasketItem(quantity, catalogItemId, unitPrice));
                return;
            }
            var existingItem = Items.FirstOrDefault(e => e.CatalogItemId == catalogItemId);
            existingItem.AddQuantity(quantity);
        }

        public void RemoveEmptyItems()
        {
            _items.RemoveAll(e => e.Quantity == 0);
        }
    }
}
