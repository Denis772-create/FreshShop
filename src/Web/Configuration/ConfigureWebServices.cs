using Web.Interfaces;
using Web.Services;

namespace Web.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogViewModelService>();
            services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();
            services.AddScoped<IBasketViewModelService, BasketViewModelService>();
            return services;
        }
    }
}
