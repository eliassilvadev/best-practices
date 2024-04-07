using Best.Practices.Core.Domain.Cqrs.CommandProvider;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Domain.Repositories.Interfaces;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Domain.Repositories
{
    public abstract class Repository<Entity> : IRepository<Entity> where Entity : IBaseEntity
    {
        private readonly ICqrsCommandProvider<Entity> _commandProvider;

        protected Repository(
            ICqrsCommandProvider<Entity> commandProvider)
        {
            _commandProvider = commandProvider;
        }
        public virtual bool Persist(Entity entity, IUnitOfWork unitOfWork)
        {
            switch (entity.State)
            {
                case EntityState.New:
                    {
                        unitOfWork.AddComand(_commandProvider.GetAddCommand(entity));
                        break;
                    };
                case EntityState.Unchanged:
                case EntityState.Persisted:
                    {
                        break;
                    };
                case EntityState.Updated:
                    {
                        unitOfWork.AddComand(_commandProvider.GetUpdateCommand(entity));
                        break;
                    };
                case EntityState.Deleted:
                case EntityState.PersistedDeleted:
                    {
                        unitOfWork.AddComand(_commandProvider.GetDeleteCommand(entity));
                        break;
                    };
            };

            return true;
        }

        public virtual Entity GetById(Guid id)
        {
            return HandleBeforeGetFromCommandProvider(_commandProvider.GetById(id));
        }

        protected virtual T HandleBeforeGetFromCommandProvider<T>(T entity) where T : IBaseEntity
        {
            if (entity is null)
                return default;

            entity.SetStateAsUnchanged();

            return entity;
        }

        protected virtual void HandleBeforeGetFromCommandProvider(IList<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Entity entityToHandle = entity;

                entityToHandle = HandleBeforeGetFromCommandProvider(entityToHandle);
            }
        }

        protected virtual void HandleEntitiesBeforeGetFromCommandProvider<T>(IList<T> entities) where T : IBaseEntity
        {
            foreach (var entity in entities)
            {
                T entityToHandle = entity;

                entityToHandle = HandleBeforeGetFromCommandProvider(entityToHandle);
            }
        }
    }
}