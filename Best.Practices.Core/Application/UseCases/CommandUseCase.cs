using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Application.UseCases
{
    public abstract class CommandUseCase<Input, Output> : BaseUseCase<Input, Output>
    {
        public delegate Entity QueryMethod<InType, Entity>(InType inputValue);
        protected IUnitOfWork UnitOfWork { get; }
        protected abstract string SaveChangesErrorMessage { get; }
        public CommandUseCase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected static void ThrowsIfEntityAlreadyExists<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string message)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            Entity entity = queryMethod.Invoke(inputValue);

            if (entity is not null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), new object[] { message });
        }

        protected static void ThrowsIfEntityDoesNotExists<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string message, out Entity entity)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            entity = queryMethod.Invoke(inputValue);

            if (entity is null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), new object[] { message });
        }

        protected static void ThrowsResourceNotFoundIfEntityDoesNotExists<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inValue, string message, out Entity entity)
            where Entity : IBaseEntity
        {
            ThrowsIfEntityDoesNotExists<InType, Entity, ResourceNotFoundException>(queryMethod, inValue, message, out entity);
        }

        protected static void ThrowsInvalidInputIfEntityExists<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string message)
            where Entity : IBaseEntity
        {
            ThrowsIfEntityAlreadyExists<InType, Entity, InvalidInputException>(queryMethod, inputValue, message);
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
