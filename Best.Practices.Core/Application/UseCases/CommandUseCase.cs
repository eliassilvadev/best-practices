using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Application.UseCases
{
    public abstract class CommandUseCase<Input, Output>(IUnitOfWork unitOfWork) : BaseUseCase<Input, Output>
    {
        public delegate Entity QueryMethod<InType, Entity>(InType inputValue);
        protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
        protected abstract string SaveChangesErrorMessage { get; }

        protected static void ThrowsIfEntityAlreadyExists<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            Entity entity = queryMethod.Invoke(inputValue);

            if (entity is not null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), [errorMessage]);
        }

        protected static void ThrowsIfEntityDoesNotExists<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage, out Entity entity)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            entity = queryMethod.Invoke(inputValue);

            if (entity is null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), [errorMessage]);
        }

        protected static void ThrowsResourceNotFoundIfEntityDoesNotExists<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inValue, string errorMessage, out Entity entity)
            where Entity : IBaseEntity
        {
            ThrowsIfEntityDoesNotExists<InType, Entity, ResourceNotFoundException>(queryMethod, inValue, errorMessage, out entity);
        }

        protected static void ThrowsInvalidInputIfEntityExists<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage)
            where Entity : IBaseEntity
        {
            ThrowsIfEntityAlreadyExists<InType, Entity, InvalidInputException>(queryMethod, inputValue, errorMessage);
        }

        protected virtual void SaveChanges()
        {
            if (!UnitOfWork.SaveChanges())
            {
                UnitOfWork.Rollback();

                throw new ExecutionErrorException(SaveChangesErrorMessage);
            }
        }
    }
}
