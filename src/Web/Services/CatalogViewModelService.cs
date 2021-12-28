using AppCore.Entities;
using AppCore.Specifications;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedKernel.Interfaces;
using System.Linq;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class CatalogViewModelService : ICatalogViewModelService
    {
        private readonly ILogger<CatalogViewModelService> _logger;
        private readonly IRepository<CatalogItem> _catalogRepository;
        private readonly IRepository<CatalogType> _catalogTypeRepository;

        public CatalogViewModelService(
            ILogger<CatalogViewModelService> logger, 
            IRepository<CatalogItem> catalogRepository,
            IRepository<CatalogType> catalogTypeRepository)
        {
            _logger = logger;
            _catalogRepository = catalogRepository;
            _catalogTypeRepository = catalogTypeRepository;
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? typeId)
        {
            _logger.LogInformation("GetCatalogItems called.");

            var filterSpecification = new CatalogFilterSpecification(typeId);
            var filterPaginatedSpecification = new CatalogFilterPaginatedSpecification(pageIndex * itemsPage, itemsPage, typeId);

            var itemsOnPage = await _catalogRepository.ListAsync(filterPaginatedSpecification);
            var totalItems = await _catalogRepository.CountAsync(filterSpecification);

            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    PictureUrl = i.PictureUri,
                    Price = i.Price
                }).ToList(),
                Types = (await GetTypes()).ToList(),
                TypesFilterApplied = typeId ?? 0,
                PaginationInfo = new PaginationInfoViewModel()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = itemsOnPage.Count,
                    TotalItems = totalItems,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems/itemsPage)).ToString()),
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            _logger.LogInformation("GetTypes called.");

            var types = await _catalogTypeRepository.ListAsync();

            var items = types
                .Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Type })
                .OrderBy(t => t.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "All" };
            items.Insert(0, allItem);
                
            return items;
        }
    }
}
