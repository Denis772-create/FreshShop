using AppCore.Entities.BasketAggregate;
using AppCore.Extensions;
using AppCore.Interfaces;
using AppCore.Specifications;
using Ardalis.GuardClauses;
using SharedKernel.Interfaces;

namespace AppCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<Basket> _repository;
        private readonly IAppLogger<BasketService> _logger;

        public BasketService(IRepository<Basket> repository,
            IAppLogger<BasketService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Basket> AddItemToBasket(string userName, int catalogItemId, decimal price, int quantity = 1)
        {
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = await _repository.GetBySpecAsync(basketSpec);

            if (basket == null)
            {
                basket = new Basket(userName);
                await _repository.AddAsync(basket);
            }

            basket.AddItem(catalogItemId, price, quantity);
            await _repository.UpdateAsync(basket);
            return basket;
        }

        public async Task DeleteBasketAsync(int basketId)
        {
            var basket = await _repository.GetByIdAsync(basketId);
            await _repository.DeleteAsync(basket);
        }

        public async Task<Basket> SetQuantities(int basketId, Dictionary<string, int> quantities)
        {
            Guard.Against.Null(quantities, nameof(quantities));
            var basketSpec = new BasketWithItemsSpecification(basketId);
            var basket = await _repository.GetBySpecAsync(basketSpec);
            Guard.Against.NullBasket(basketId, basket);

            foreach (var item in basket.Items)
            {
                if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
                {
                    if (_logger != null) _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                    item.SetQuantity(quantity);
                }
            }
            basket.RemoveEmptyItems();
            await _repository.UpdateAsync(basket);
            return basket;
        }

        public async Task TransferBasketAsync(string anonymousId, string userName)
        {
            Guard.Against.NullOrEmpty(anonymousId, nameof(anonymousId));
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
            var anonymousBasket = await _repository.GetBySpecAsync(anonymousBasketSpec);
            if (anonymousBasket == null) return;
            var userBasketSpec = new BasketWithItemsSpecification(userName);
            var userBasket = await _repository.GetBySpecAsync(userBasketSpec);
            if (userBasket == null)
            {
                userBasket = new Basket(userName);
                await _repository.AddAsync(userBasket);
            }
            foreach (var item in anonymousBasket.Items)
            {
                userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
            }
            await _repository.UpdateAsync(userBasket);
            await _repository.DeleteAsync(anonymousBasket);


        }
    }
}
