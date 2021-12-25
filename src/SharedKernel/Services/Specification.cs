using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace SharedKernel.Services
{
    public class Specification<T> : ISpecification<T>
    {
        protected virtual ISpecificationBuilder<T> Query { get; }
        public Specification()
        {
            Query = new SpecificationBuilder<T>(this);
        }
        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();

        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; } =
            new List<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)>();

        public IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; } = new List<IncludeExpressionInfo>();

        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        public IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; } =
            new List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>();
        public bool IsPagingEnabled { get; set; } = false;
        public int? Take { get;  set; } = null;

        public int? Skip { get;  set; } = null;

        public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; internal set; } = null;

        public bool CacheEnabled { get;  set; }

        public string? CacheKey { get;  set; }

        public bool AsNoTracking { get;  set; } = false;
        public bool AsSplitQuery { get;  set; } = false;

        public bool AsNoTrackingWithIdentityResolution { get;  set; } = false;
    }

}
