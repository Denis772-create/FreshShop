using SharedKernel.Interfaces;

namespace SharedKernel.Services
{
    public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty>, ISpecificationBuilder<T> where T : class
    {
        public Specification<T> Specification { get; }
        public IncludableSpecificationBuilder(Specification<T> specification)
        {
            Specification = specification;
        }
    }
}
