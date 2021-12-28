using AppCore.Entities;
using AppCore.Extensions;
using SharedKernel.Services;

namespace AppCore.Specifications
{
    public class CatalogFilterSpecification : Specification<CatalogItem>
    {
        public CatalogFilterSpecification(int? typeId)
        {
            Query
                .Where(i => !typeId.HasValue || i.CatalogTypeId == typeId);
        }
    }
}
