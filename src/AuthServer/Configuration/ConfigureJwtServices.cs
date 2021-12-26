using AuthServer.Interfaces;
using AuthServer.Services;

namespace AuthServer.Configuration
{
    public static class ConfigureJwtServices
    {
        public static IServiceCollection AddJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthConfiguration>(configuration.GetSection("Authentication"));
            services.AddSingleton<TokenManager>();
            services.AddScoped<Authenticator>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
