using SharedKernel.Interfaces;

namespace SharedKernel.Services
{
    public class SpecificationBuilder<T> : ISpecificationBuilder<T>
    {
        public Specification<T> Specification { get; }
        public SpecificationBuilder(Specification<T> specification)
        {
            Specification = specification;
        }
    }
}
