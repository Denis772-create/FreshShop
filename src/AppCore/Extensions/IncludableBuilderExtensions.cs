using SharedKernel;
using SharedKernel.Interfaces;
using SharedKernel.Services;
using System.Linq.Expressions;

namespace AppCore.Extensions
{
    public static class IncludableBuilderExtensions
    {
        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder, 
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression) where TEntity : class
        {
            var item = new IncludeExpressionInfo(thenIncludeExpression, 
                                                typeof(TEntity), 
                                                typeof(TProperty), 
                                                typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(item);

            return new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
        }

        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder, 
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression) where TEntity : class
        {
            var item = new IncludeExpressionInfo(thenIncludeExpression, 
                                                typeof(TEntity), 
                                                typeof(TProperty), 
                                                typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(item);

            return new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
        }

    }
}
