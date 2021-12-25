using AppCore.Entities.BasketAggregate;
using SharedKernel.Services;
using AppCore.Extensions;

namespace AppCore.Specifications
{
    public class BasketWithItemsSpecification : Specification<Basket>
    { 
        public BasketWithItemsSpecification(int basketId)
        {
            Query.Where(b => b.Id == basketId)
                  .Include(b => b.Items);
        }
    }
}
