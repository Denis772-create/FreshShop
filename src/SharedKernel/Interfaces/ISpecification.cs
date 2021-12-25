using SharedKernel;
using System.Linq.Expressions;

namespace SharedKernel.Interfaces
{
    public enum OrderTypeEnum
    {
        OrderBy = 1,
        OrderByDescending = 2,
        ThenBy = 3,
        ThenByDescending = 4
    }
    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }
        IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; }
        IEnumerable<string> IncludeStrings { get; }
        IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; }
        int? Take { get; }
        int? Skip { get; }
        Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; }
        bool CacheEnabled { get; }
        string? CacheKey { get; }
        bool AsNoTracking { get; }
        bool AsSplitQuery { get; }
        bool AsNoTrackingWithIdentityResolution { get; }
        // IEnumerable<T> Evaluate(IEnumerable<T> entities);

    }
}
