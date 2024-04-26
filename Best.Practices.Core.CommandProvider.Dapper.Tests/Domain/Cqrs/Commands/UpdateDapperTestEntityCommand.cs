using Best.Practices.Core.CommandProvider.Dapper.EntityCommands;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions;
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

            var updateCommandDefinition = UpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(
                entity, DapperTestEntityTableDefinition.TableColumnDefinitions, "EntityTestTable", "Id");

            commandDefinitions.Add(updateCommandDefinition);

            foreach (var child in entity.Childs.AllItems)
            {
                if (child.State == Core.Domain.Enumerators.EntityState.New)
                {
                    var updateChildCommandDefinition = InsertCommandFromEntityProperties(
                        child, DapperChildEntityTestTableDefinition.TableColumnDefinitions, "ChildEntityTestTable");

                    commandDefinitions.Add(updateChildCommandDefinition);
                }

                if (child.State == Core.Domain.Enumerators.EntityState.Updated)
                {
                    var updateChildCommandDefinition = UpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(
                        child, DapperChildEntityTestTableDefinition.TableColumnDefinitions, "ChildEntityTestTable", "Id");

                    commandDefinitions.Add(updateChildCommandDefinition);
                }

                if (child.State == Core.Domain.Enumerators.EntityState.Deleted)
                {
                    var updateChildCommandDefinition = DeleteCommandWithEntityAndIdCriteria(
                        child, DapperChildEntityTestTableDefinition.TableColumnDefinitions, "ChildEntityTestTable");

                    commandDefinitions.Add(updateChildCommandDefinition);
                }
            }

            return commandDefinitions;
        }
    }
}