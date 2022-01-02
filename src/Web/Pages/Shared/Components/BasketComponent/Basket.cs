using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Pages.Shared.Components.BasketComponent
{
    public class Basket : ViewComponent
    {
        private readonly IBasketViewModelService _basketService;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        public Basket(IBasketViewModelService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new BasketComponentViewModel
            {
                ItemsCount = await CountTotalBasketItems()
            };
            return View(vm);  
        }

        private async Task<int> CountTotalBasketItems()
        {
            //todo identity service

            string anonymousId = GetAnnonymousIdFromCookie();
            if (anonymousId == null)
                return 0;

            return await _basketService.CountTotalBasketItems(anonymousId);
        }

        private string? GetAnnonymousIdFromCookie()
        {
            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                var id = Request.Cookies[Constants.BASKET_COOKIENAME];

                if (Guid.TryParse(id, out var _))
                    return id;
            }
            return null;
        }
    }
}
