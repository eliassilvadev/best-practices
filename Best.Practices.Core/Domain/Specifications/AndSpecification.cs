using Best.Practices.Core.Domain.Specifications.Interfaces;

namespace Best.Practices.Core.Domain.Specifications
{
    public class AndSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _one;
        private readonly ISpecification<T> _other;
        public AndSpecification(ISpecification<T> one, ISpecification<T> other)
        {
            _one = one;
            _other = other;
        }

        public override bool IsSatisfiedBy(T instance)
        {
            return _one.IsSatisfiedBy(instance) && _other.IsSatisfiedBy(instance);
        }
    }
}