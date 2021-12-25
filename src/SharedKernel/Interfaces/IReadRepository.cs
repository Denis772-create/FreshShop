namespace SharedKernel.Interfaces
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> GetByIdAsync<TId>(TId entity, CancellationToken token = default) where TId: notnull;
        Task<T?> GetBySpecAsync<Spec>(Spec specification, CancellationToken token = default) where Spec : ISpecification<T>;
        Task<List<T>> ListAsync(CancellationToken token = default);
        Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken token = default);
        Task<int> CountAsync(CancellationToken token = default);
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);   
    }
}
