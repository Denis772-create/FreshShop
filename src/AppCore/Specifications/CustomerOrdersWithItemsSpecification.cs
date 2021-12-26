using AppCore.Entities.OrderAggregate;
using AppCore.Extensions;
using SharedKernel.Services;
    
namespace AppCore.Specifications
{
    public class CustomerOrdersWithItemsSpecification : Specification<Order>
    {
        public CustomerOrdersWithItemsSpecification(string buyerId)
        {
            Query.Where(c => c.BuyerId == buyerId)
                .Include(c => c.Items)
                .ThenInclude(i => i.ItemOrdered);
                
        }
    }
}
