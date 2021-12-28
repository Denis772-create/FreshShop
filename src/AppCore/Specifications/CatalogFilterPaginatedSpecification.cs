using AppCore.Entities;
using AppCore.Extensions;
using SharedKernel.Services;

namespace AppCore.Specifications
{
    public class CatalogFilterPaginatedSpecification : Specification<CatalogItem>
    {
        public CatalogFilterPaginatedSpecification(int skip, int take, int? typeId)
        {
            if (take == 0)
                take = int.MaxValue;
            
            Query
                .Where(i => !typeId.HasValue || i.CatalogTypeId == typeId)
                .Skip(skip)
                .Take(take);
        }
    }
}
