using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Domain.Models.Interfaces;
using Dapper;
using System.Data.Common;

namespace Best.Practices.Core.CommandProvider.Dapper.CommandProviders
{
    public abstract class DapperCqrsCommandProvider<Entity> : ICqrsCommandProvider<Entity> where Entity : IBaseEntity
    {
        protected DbConnection Connection { get; }
        protected IList<DapperTableColumnDefinitions> TableColumnDefinitions { get; set; }

        public DapperCqrsCommandProvider(DbConnection connection)
        {
            Connection = connection;
        }

        public abstract Entity GetById(Guid id);

        public abstract IEntityCommand GetAddCommand(Entity entity);

        public abstract IEntityCommand GetDeleteCommand(Entity entity);

        public abstract IEntityCommand GetUpdateCommand(Entity entity);

        protected void AddStringNullableParameter(DynamicParameters parameters, string parameterName, object parameterValue, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                parameters.Add(parameterName, DBNull.Value, size: size);
            else
                parameters.Add(parameterName, parameterValue, size: size);
        }
    }
}