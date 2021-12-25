using System.Linq.Expressions;

namespace SharedKernel
{
    public enum IncludeTypeEnum
    {
        Include = 1,
        ThenInclude = 2
    }
    public class IncludeExpressionInfo
    {
        public LambdaExpression LambdaExpression { get; }
        public Type EntityType { get; }
        public Type PropertyType { get; }
        public Type? PreviousPropertyType { get; }
        public IncludeTypeEnum Type { get; }

        public IncludeExpressionInfo(LambdaExpression lambdaExpression,
                                     Type entityType, 
                                     Type propertyType, 
                                     Type? previousPropertyType, 
                                     IncludeTypeEnum type)
        {
            _ = lambdaExpression ?? throw new ArgumentNullException(nameof(lambdaExpression));
            _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
            _ = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

            if (type == IncludeTypeEnum.ThenInclude)
            {
                _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
            }

            LambdaExpression = lambdaExpression;
            EntityType = entityType;
            PropertyType = propertyType;
            PreviousPropertyType = previousPropertyType;
            Type = type;
        }

        public IncludeExpressionInfo(LambdaExpression expression,
                             Type entityType,
                             Type propertyType)
    : this(expression, entityType, propertyType, null, IncludeTypeEnum.Include)
        {
        }

        public IncludeExpressionInfo(LambdaExpression expression,
                                     Type entityType,
                                     Type propertyType,
                                     Type previousPropertyType)
            : this(expression, entityType, propertyType, previousPropertyType, IncludeTypeEnum.ThenInclude)
        {
        }
    }
}
