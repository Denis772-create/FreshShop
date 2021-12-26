using AppCore.Entities;
using AppCore.Extensions;
using SharedKernel.Services;

namespace AppCore.Specifications
{
    public class CatalogItemsSpecification : Specification<CatalogItem>
    {
        public CatalogItemsSpecification(params int[] ids)
        {
            Query.Where(c => ids.Contains(c.Id));
        }
    }
}
