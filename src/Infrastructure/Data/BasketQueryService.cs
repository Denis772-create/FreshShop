using AppCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BasketQueryService : IBasketQueryService
    {
        private readonly CatalogContext _catalogContext;

        public BasketQueryService(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<int> CountTotalBasketItems(string userName)
        {
            return await _catalogContext.Baskets
                .Where(b=> b.BuyerId == userName)
                .SelectMany(i => i.Items)
                .SumAsync(c => c.Quantity);
        }
    }
}
