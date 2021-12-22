using AppCore.Interfaces;

namespace AppCore.Entities.BuyerAggregate
{
    public class Buyer : BaseEntity, IAggregateRoot
    {
        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();
        public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();
    }
}
