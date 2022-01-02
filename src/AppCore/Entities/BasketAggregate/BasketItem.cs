using Ardalis.GuardClauses;

namespace AppCore.Entities.BasketAggregate
{
    public class BasketItem : BaseEntity
    {
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; } = 1;
        public int CatalogItemId { get; private set; }
        public int BasketId { get; private set; }
        public BasketItem(int quantity, int catalogItemId, decimal unitPrice)
        {
            CatalogItemId = catalogItemId;
            UnitPrice = unitPrice;
        }

        public void AddQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity += quantity;
        }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity),0, int.MaxValue);

            Quantity = quantity;
        }
    }
}
