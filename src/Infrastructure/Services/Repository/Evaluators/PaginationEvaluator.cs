using SharedKernel.Interfaces;

namespace Infrastructure.Services.Repository.Evaluators
{
    public class PaginationEvaluator : IEvaluator
    {
        private PaginationEvaluator() { }
        public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.Skip != null && specification.Skip != 0)
                query = query.Skip(specification.Skip.Value);

            if (specification.Take != null && specification.Take != 0)
                query = query.Take(specification.Take.Value);

            return query;
        }
    }
}
