using SharedKernel;
using SharedKernel.Interfaces;
using SharedKernel.Services;
using System.Linq.Expressions;

namespace AppCore.Extensions
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> Where<T>(this ISpecificationBuilder<T> builder,
                                                        Expression<Func<T, bool>> criteria)
        {
            ((List<Expression<Func<T, bool>>>)builder.Specification.WhereExpressions).Add(criteria);
            return builder;
        }

        public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
                            this ISpecificationBuilder<T> specificationBuilder,
                            Expression<Func<T, TProperty>> includeExpression) where T : class
        {
            var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));

            ((List<IncludeExpressionInfo>)specificationBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification);

            return includeBuilder;
        }


        public static ISpecificationBuilder<T> Include<T>(this ISpecificationBuilder<T> specificationBuilder,
                                                          string includeString) where T : class
        {
            ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Take<T>(this ISpecificationBuilder<T> specificationBuilder, int take)
        {
            if (specificationBuilder.Specification.Take != null) throw new Exception();

            specificationBuilder.Specification.Take = take;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Skip<T>(this ISpecificationBuilder<T> specificationBuilder, int skip)
        {
            if (specificationBuilder.Specification.Skip != null) throw new Exception();

            specificationBuilder.Specification.Skip = skip;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }
    }
}
