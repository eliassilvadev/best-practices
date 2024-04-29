using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Domain.Models.Interfaces;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.CommandProviders
{
    public abstract class DapperCqrsCommandProvider<Entity> : ICqrsCommandProvider<Entity> where Entity : IBaseEntity
    {
        protected IDbConnection _connection;

        public DapperCqrsCommandProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public abstract Task<Entity> GetById(Guid id);

        public abstract IEntityCommand GetAddCommand(Entity entity);

        public abstract IEntityCommand GetDeleteCommand(Entity entity);

        public abstract IEntityCommand GetUpdateCommand(Entity entity);
    }
}