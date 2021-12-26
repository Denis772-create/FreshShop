using AppCore.Entities.BasketAggregate;
using Ardalis.GuardClauses;
using SharedKernel.Exceptions;

namespace AppCore.Extensions
{
    public static class GuardExtensions
    {
        public static void NullBasket(this IGuardClause guardClause, int basketId, Basket basket)
        {
            if (basket == null)
                throw new BasketNotFoundException(basketId);
        }

        public static void EmptyBasketOnCheckout(this IGuardClause guardClause, IReadOnlyCollection<BasketItem> basketItems)
        {
            if (!basketItems.Any())
                throw new EmptyBasketOnCheckoutException();
        }

    }
}
