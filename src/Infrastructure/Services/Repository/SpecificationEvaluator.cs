using Infrastructure.Services.Repository.Evaluators;
using SharedKernel.Interfaces;

namespace Infrastructure.Services.Repository
{
    public class SpecificationEvaluator : ISpecificationEvaluator
    {
        public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();
        private readonly List<IEvaluator> evaluators = new List<IEvaluator>();

        public SpecificationEvaluator()
        {
            this.evaluators.AddRange(new IEvaluator[]
            {
                WhereEvaluator.Instance,
                SearchEvaluator.Instance,
                IncludeEvaluator.Instance,
                PaginationEvaluator.Instance
            });
        }
        public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }

        public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
        {
            var evaluators = evaluateCriteriaOnly ? this.evaluators.Where(x=>x.IsCriteriaEvaluator) : this.evaluators;

            foreach (var evaluator in evaluators)
            inputQuery = evaluator.GetQuery(inputQuery, specification);

            return inputQuery;
        }
    }
}
