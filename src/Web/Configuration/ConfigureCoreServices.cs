using AppCore.Interfaces;
using AppCore.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Repository;
using SharedKernel.Interfaces;

namespace Web.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBasketQueryService, BasketQueryService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(configuration);
            return services;
        }
    }
}
