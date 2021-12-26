namespace SharedKernel.Interfaces
{
    public interface IIncludableSpecificationBuilder<T,out TProperty> : ISpecificationBuilder<T> where T : class
    {

    }
}
