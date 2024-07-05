using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Interceptors;
using Best.Practices.Core.Domain.Entities.Interfaces;
using Best.Practices.Core.Domain.Repositories.Interfaces;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Domain.Repositories
{
    public abstract class Repository<Entity> : IRepository<Entity> where Entity : IBaseEntity
    {
        private readonly ICqrsCommandProvider<Entity> _commandProvider;

        private readonly Dictionary<EntityState, Func<Entity, IEntityCommand>> _persistenceMethods = [];

        protected Repository(
            ICqrsCommandProvider<Entity> commandProvider)
        {
            _commandProvider = commandProvider;

            _persistenceMethods[EntityState.New] = _commandProvider.GetAddCommand;
            _persistenceMethods[EntityState.Updated] = _commandProvider.GetUpdateCommand;
            _persistenceMethods[EntityState.Deleted] = _commandProvider.GetDeleteCommand;
            _persistenceMethods[EntityState.PersistedDeleted] = _commandProvider.GetDeleteCommand;
        }

        public virtual void Persist(Entity entity, IUnitOfWork unitOfWork)
        {
            var persistenceMethod = _persistenceMethods.GetValueOrDefault(entity.State);

            if (persistenceMethod is not null)
                unitOfWork.AddComand(persistenceMethod(entity));
        }

        public virtual async Task<Entity> GetById(Guid id)
        {
            return HandleAfterGetFromCommandProvider(await _commandProvider.GetById(id));
        }

        protected virtual T HandleAfterGetFromCommandProvider<T>(T entity) where T : IBaseEntity
        {
            if (entity is null)
                return default;

            entity.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptorLinfu(entity);

            return interceptor.CreateEntityWihStateControl(entity);
        }
    }
}