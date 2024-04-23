using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Models.Interfaces;
using System.Data.Common;

namespace Best.Practices.Core.CommandProvider.Dapper.EntityCommands
{
    public abstract class DapperCommand<Entity> : IEntityCommand where Entity : IBaseEntity
    {
        protected readonly DbConnection _connection;
        protected IList<DapperCommandDefinition> CommandDefinitions { get; }

        public IBaseEntity AffectedEntity { get; }
        public DapperCommand(DbConnection connection, Entity affectedEntity)
        {
            _connection = connection;
            AffectedEntity = affectedEntity;
            CommandDefinitions = new List<DapperCommandDefinition>();
        }

        public virtual bool Execute()
        {
            bool sucess;
            try
            {
                var commandDefinitions = AddCommandDefinitions((Entity)AffectedEntity);

                foreach (var commandDefinition in commandDefinitions)
                {
                    commandDefinition.Execute(_connection);
                }

                sucess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                sucess = false;
            }

            return sucess;
        }

        public abstract IList<DapperCommandDefinition> AddCommandDefinitions(Entity entity);
    }
}