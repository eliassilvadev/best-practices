namespace Best.Practices.Core.Domain.Specifications.Interfaces
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public bool Includes(ISpecification<T> other)
        {
            return false;
        }

        public abstract bool IsSatisfiedBy(T instance);

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }
    }
}