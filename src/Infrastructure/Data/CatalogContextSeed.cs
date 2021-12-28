using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext context,
            ILoggerFactory loggerFactory, int retry = 0)
        {
            var retryForAvailability = retry;

            try
            {
                if (context.Database.IsSqlServer())
                    context.Database.Migrate();

                if (!await context.CatalogTypes.AnyAsync())
                {
                    await context.CatalogTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());
                    await context.SaveChangesAsync();
                }

                if (!await context.CatalogItems.AnyAsync())
                {
                    await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;
                var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                log.LogError(e.Message);
                await SeedAsync(context, loggerFactory, retry);
                throw;
            }
        }
        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new("Type1"),
                new("Type2"),
                new("Type3"),
                new("Type4")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new(2,".NET Bot Black Sweatshirt" , ".NET Bot Black Sweatshirt" , 19.5M , "banner-02.jpg"),
                new(1,".NET Black & White Mug"    , ".NET Black & White Mug"    , 8.50M , "big-img-01.jpg"),
                new(2,"Prism White T-Shirt"       , "Prism White T-Shirt"       , 12    , "big-img-01.jpg"),
                new(2,".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", 12    , "big-img-01.jpg"),
                new(3,"Roslyn Red Sheet"          , "Roslyn Red Sheet"          , 8.5M  , "big-img-01.jpg"),
                new(2,".NET Blue Sweatshirt"      , ".NET Blue Sweatshirt"      , 12    , "big-img-01.jpg"),
                new(2,"Roslyn Red T-Shirt"        , "Roslyn Red T-Shirt"        , 12    , "big-img-01.jpg"),
                new(2,"Kudu Purple Sweatshirt"    , "Kudu Purple Sweatshirt"    , 8.5M  , "big-img-01.jpg"),
                new(1,"Cup<T> White Mug"          , "Cup<T> White Mug"          , 12    , "big-img-01.jpg"),
                new(3,".NET Foundation Sheet"     , ".NET Foundation Sheet"     , 12    , "big-img-01.jpg"),
                new(3,"Cup<T> Sheet"              , "Cup<T> Sheet"              , 8.5M  , "big-img-01.jpg"),
                new(2,"Prism White TShirt"        , "Prism White TShirt"        , 12    , "big-img-01.jpg")
            };
        }
    }
}
