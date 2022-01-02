using AppCore.Entities;
using AppCore.Entities.BasketAggregate;
using AppCore.Interfaces;
using AppCore.Specifications;
using SharedKernel.Interfaces;
using Web.Interfaces;
using Web.Pages.Basket;

namespace Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IBasketQueryService _basketQueryService;

        public BasketViewModelService(IBasketQueryService basketQueryService, 
            IRepository<CatalogItem> itemRepository, 
            IRepository<Basket> basketRepository)
        {
            _basketQueryService = basketQueryService;
            _itemRepository = itemRepository;
            _basketRepository = basketRepository;
        }

        public Task<int> CountTotalBasketItems(string username)
        {
            var counter = _basketQueryService.CountTotalBasketItems(username);
            return counter;
        }

        public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
        {
            var baskSpec = new BasketWithItemsSpecification(userName);
            var basket = await _basketRepository.GetBySpecAsync(baskSpec);

            if (basket == null)
                return await CreateBasketForUser(userName);

            return await Map(basket);
        }

        private async Task<BasketViewModel> CreateBasketForUser(string username)
        {
            var basket = new Basket(username);
            await _basketRepository.AddAsync(basket);

            return new BasketViewModel
            {
                BuyerId = basket.BuyerId,
                Id = basket.Id
            };
        }

        public async Task<BasketViewModel> Map(Basket basket)
        {
            return new BasketViewModel
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = await GetBasketItems(basket.Items)
            };
        }

        private async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<BasketItem> basketItems)
        {
            var catalogItemsSpecification = new CatalogItemsSpecification(basketItems.Select(i => i.CatalogItemId).ToArray());
            var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

            var items = basketItems.Select(bi =>
            {
                var catalogItem = catalogItems.FirstOrDefault(i => i.Id == bi.CatalogItemId);

                var basketItemViewModel = new BasketItemViewModel
                {
                    Id = bi.Id,
                    UnitPrice = bi.UnitPrice,
                    Quantity = bi.Quantity,
                    CatalogItemId = bi.CatalogItemId,
                    ProductName = catalogItem.Name,
                    PictureUrl = catalogItem.PictureUri
                };

                return basketItemViewModel;
            }).ToList();

            return items;
        }
    }
}
