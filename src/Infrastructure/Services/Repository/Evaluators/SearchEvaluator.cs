using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Services.Repository.Evaluators
{
    public class SearchEvaluator : IEvaluator
    {
        private SearchEvaluator() { }
        public static SearchEvaluator Instance { get; } = new SearchEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
                query = query.Search(criterias);
            }

            return query;
        }
    }
    public static class SearchExtension
    {
        public static object ParameterReplacerVisitor { get; private set; }

        public static IQueryable<T> Search<T>(this IQueryable<T> source, IEnumerable<(Expression<Func<T, string>> selector, string searchTerm)> criterias)
        {
            Expression? expr = null;
            var parameter = Expression.Parameter(typeof(T), "x");

            foreach (var criteria in criterias)
            {
                if (criteria.selector == null || string.IsNullOrEmpty(criteria.searchTerm))
                    continue;

                var functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions)));
                var like = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { functions.Type, typeof(string), typeof(string) });

                var propertySelector = Evaluators.ParameterReplacerVisitor.Replace(criteria.selector, criteria.selector.Parameters[0], parameter);

                var likeExpression = Expression.Call(
                                        null,
                                        like,
                                        functions,
                                        (propertySelector as LambdaExpression)?.Body,
                                        Expression.Constant(criteria.searchTerm));

                expr = expr == null ? (Expression)likeExpression : Expression.OrElse(expr, likeExpression);
            }

            return expr == null
                ? source
                : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
        }
    }
    internal class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly Expression newExpression;
        private readonly ParameterExpression oldParameter;

        private ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression)
        {
            this.oldParameter = oldParameter;
            this.newExpression = newExpression;
        }

        internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression)
        {
            return new ParameterReplacerVisitor(oldParameter, newExpression).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (p == oldParameter)
            {
                return newExpression;
            }
            else
            {
                return p;
            }
        }
    }
}
