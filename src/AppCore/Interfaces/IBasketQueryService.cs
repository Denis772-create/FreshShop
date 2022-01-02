namespace AppCore.Interfaces
{
    public interface IBasketQueryService
    {
        Task<int> CountTotalBasketItems(string userName);
    }
}
