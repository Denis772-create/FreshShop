
using SharedKernel.Interfaces;
using System.Linq;

namespace Infrastructure.Services.Repository.Evaluators
{
    public class WhereEvaluator : IEvaluator
    {
        public WhereEvaluator() { }
        public static WhereEvaluator Instance { get; } = new WhereEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            foreach (var criteria in specification.WhereExpressions)
            {
                query = query.Where(criteria);
            }
            return query;
        }
    }
}
