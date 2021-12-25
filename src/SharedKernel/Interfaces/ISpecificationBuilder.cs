using SharedKernel.Services;

namespace SharedKernel.Interfaces
{
    public interface ISpecificationBuilder<T>
    {
        Specification<T> Specification { get; }
    }
}
