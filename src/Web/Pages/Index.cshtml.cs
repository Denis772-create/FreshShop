using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogViewModelService _catalogViewModel;

        public IndexModel(ICatalogViewModelService catalogViewModel)
        {
            _catalogViewModel = catalogViewModel;
        }

        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

        public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
        {
            CatalogModel = await _catalogViewModel.GetCatalogItems(pageId ?? 0, Constants.ITEMS_PER_PAGE, catalogModel.TypesFilterApplied);
        }
    }
}
