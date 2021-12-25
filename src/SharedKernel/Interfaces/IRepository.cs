namespace SharedKernel.Interfaces
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken token = default);
        Task UpdateAsync(T entity, CancellationToken token = default);
        Task DeleteAsync(T entity, CancellationToken token = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken token = default);
        Task SaveChangesAsync(CancellationToken token = default);
    }
}
