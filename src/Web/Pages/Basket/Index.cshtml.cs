using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Pages.Basket
{
    public class IndexModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly IBasketViewModelService _basketViewModelService;


        public IndexModel(IBasketService basketService,
            IBasketViewModelService basketViewModelService)
        {
            _basketService = basketService;
            _basketViewModelService = basketViewModelService;
        }

        public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

        public async Task OnGet()
        {
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
        }

        public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToPage("/Index");
            }

            var username = GetOrSetBasketCookieAndUserName();
            var basket = await _basketService.AddItemToBasket(username, productDetails.Id, productDetails.Price);

            BasketModel = await _basketViewModelService.Map(basket);

            return RedirectToPage();
        }

        public async Task OnPostUpdate(IEnumerable<BasketItemViewModel> basketItems)
        {
            if (!ModelState.IsValid)
                return;

            var basketView = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
            var updateModel = basketItems.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
            var basket = await _basketService.SetQuantities(basketView.Id, updateModel);
            BasketModel = await _basketViewModelService.Map(basket);
        }

        private string GetOrSetBasketCookieAndUserName()
        {
            string userName = null;

            if (User.Identity.IsAuthenticated)
                return User.Identity.Name;

            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                userName = Request.Cookies[Constants.BASKET_COOKIENAME];

                if (!User.Identity.IsAuthenticated)
                {
                    if (!Guid.TryParse(userName, out var _))
                        userName = null;
                }
            }
            if (userName != null) return userName;

            userName = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddYears(10)
            };
            Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

            return userName;
        }
    }
}
