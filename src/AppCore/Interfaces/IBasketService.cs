using AppCore.Entities.BasketAggregate;

namespace AppCore.Interfaces
{
    public interface IBasketService
    {
        Task TransferBasketAsync(string anonymousId, string userName);
        Task<Basket> AddItemToBasket(string userName,int catalogItemId,decimal price, int quantity = 1);
        Task<Basket> SetQuantities(int basketId,Dictionary<string, int> quantities);
        Task DeleteBasketAsync(int basketId);
    }
}
