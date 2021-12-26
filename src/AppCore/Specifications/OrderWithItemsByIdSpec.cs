using AppCore.Entities.OrderAggregate;
using AppCore.Extensions;
using SharedKernel.Services;

namespace AppCore.Specifications
{
    public class OrderWithItemsByIdSpec : Specification<Order>
    {
        public OrderWithItemsByIdSpec(int orderId)
        {
            Query
                .Where(o => o.Id == orderId)
                .Include(i => i.Items)
                .ThenInclude(i => i.ItemOrdered);
        }
    }
}
