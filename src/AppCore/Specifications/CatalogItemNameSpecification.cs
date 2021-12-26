using AppCore.Entities;
using AppCore.Extensions;
using SharedKernel.Services;

namespace AppCore.Specifications
{
    public class CatalogItemNameSpecification : Specification<CatalogItem>
    {
        public CatalogItemNameSpecification(string catalogItemName)
        {
            Query.Where(c => c.Name == catalogItemName);
        }
    }
}
