namespace Web.Extensions
{
    public class CacheHelpers
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}";

        public static string GenereteCatalogItemCacheKey(int pageIndex, int itemsPage, int? typeId)
        {
            return string.Format(_itemsKeyTemplate, pageIndex, itemsPage, typeId);
        }

        public static string GenerateTypesCacheKey()
        {
            return "types";
        }

    }
}
