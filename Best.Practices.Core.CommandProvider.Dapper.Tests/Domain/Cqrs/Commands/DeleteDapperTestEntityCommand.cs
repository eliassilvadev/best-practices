using Best.Practices.Core.CommandProvider.Dapper.EntityCommands;
using Best.Practices.Core.CommandProvider.Dapper.Extensions;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions;
using Dapper;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.Commands
{
    public class DeleteDapperTestEntityCommand : DapperCommand<DapperTestEntity>
    {
        public DeleteDapperTestEntityCommand(
            IDbConnection connection,
            DapperTestEntity affectedEntity
            ) : base(connection, affectedEntity)
        {
            _entityTableTypeMappings.Add(nameof(DapperTestEntity), DapperTestEntityTableDefinition.TableDefinition);
        }

        public override IList<CommandDefinition> CreateCommandDefinitions(DapperTestEntity entity)
        {
            var commandDefinitions = new List<CommandDefinition>();

            var deleteQuery = "Delete From EntityTestTable Where Id = @Id";

            var parameters = new DynamicParameters();

            parameters.AddNullable(@"Id", entity);

            var deleteCommandDefinition = new CommandDefinition(deleteQuery, parameters);

            commandDefinitions.Add(deleteCommandDefinition);

            return commandDefinitions;
        }
    }
}