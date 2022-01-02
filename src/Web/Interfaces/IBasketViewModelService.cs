using AppCore.Entities.BasketAggregate;
using Web.Pages.Basket;

namespace Web.Interfaces
{
    public interface IBasketViewModelService
    {
        Task<BasketViewModel> GetOrCreateBasketForUser(string userName);
        Task<int> CountTotalBasketItems(string username);
        Task<BasketViewModel> Map(Basket basket);
    }
}
