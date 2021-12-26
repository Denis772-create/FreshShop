using AuthServer;
using AuthServer.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        using (IServiceScope scope = host.Services.CreateScope())
        {
            using (AuthDbContext context = scope.ServiceProvider.GetRequiredService<AuthDbContext>())
            {
                context.Database.Migrate();
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}