using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Domain.Cqrs.CommandProviders
{
    internal class CqrsCommandProviderInMemory<Entity> : ICqrsCommandProvider<Entity> where Entity : IBaseEntity
    {
        private readonly InMemoryConnection<Entity> _connection;

        public CqrsCommandProviderInMemory(InMemoryConnection<Entity> connection)
        {
            _connection = connection;
        }

        public IEntityCommand GetAddCommand(Entity entity)
        {
            IList<Entity> entities = GetEntityList();

            entities.Add(entity);
        }

        public Task<Entity> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected IList<Entity> GetEntityList()
        {
            IList<Entity> entidades = _connection.PersistedEntities[typeof(Entity).Name];

            return entidades;
        }

        public IEntityCommand GetDeleteCommand(Entity entity)
        {
            IList<Entity> entities = GetEntityList();

            Entity previousEntity = entities.Where(e => e.Id == entity.Id).FirstOrDefault();

            if (previousEntity is not null)
            {
                entities.Remove(previousEntity);
            }

            return true;
        }

        public IEntityCommand GetUpdateCommand(Entity entity)
        {
            IList<Entity> entities = GetEntityList();

            Entity previousEntity = entities.Where(e => e.Id == entity.Id).FirstOrDefault();

            if ((previousEntity is not null) && (previousEntity.Equals(entity)))
            {
                previousEntity.Copy(entity);
            }

            return true;
        }
    }
}
