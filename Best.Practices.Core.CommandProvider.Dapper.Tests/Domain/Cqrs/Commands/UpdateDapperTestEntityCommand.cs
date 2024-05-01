using Best.Practices.Core.CommandProvider.Dapper.EntityCommands;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions;
using Best.Practices.Core.Domain.Models.Interfaces;
using Dapper;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.Commands
{
    public class UpdateDapperTestEntityCommand : DapperCommand<DapperTestEntity>
    {
        public UpdateDapperTestEntityCommand(
            IDbConnection connection,
            DapperTestEntity affectedEntity
            ) : base(connection, affectedEntity)
        {
        }

        public override IList<CommandDefinition> GetCommandDefinitions(DapperTestEntity entity)
        {
            var commandDefinitions = base.GetCommandDefinitions(entity);

            var updateCommandDefinition = GetCommandDefinitionByState(entity, DapperTestEntityTableDefinition.TableDefinition);

            if (updateCommandDefinition.HasValue)
                commandDefinitions.Add(updateCommandDefinition.Value);

            foreach (var child in entity.Childs.AllItems)
            {
                var commandDefinition = GetCommandDefinitionByState(
                    child,
                    DapperChildEntityTestTableDefinition.TableDefinition,
                    new Dictionary<string, IBaseEntity>() { { "ParentEntity", entity } });

                if (commandDefinition.HasValue)
                    commandDefinitions.Add(commandDefinition.Value);
            }

            return commandDefinitions;
        }
    }
}