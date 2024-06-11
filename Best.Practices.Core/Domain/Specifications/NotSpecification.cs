using Best.Practices.Core.Domain.Specifications.Interfaces;

namespace Best.Practices.Core.Domain.Specifications
{
    public class NotSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _one;
        public NotSpecification(ISpecification<T> one)
        {
            _one = one;
        }

        public override bool IsSatisfiedBy(T instance)
        {
            return !_one.IsSatisfiedBy(instance);
        }
    }
}