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

        public override IList<CommandDefinition> CreateCommandDefinitions(DapperTestEntity entity)
        {
            base.CreateCommandDefinitions(entity);

            CreateCommandDefinitionByState(entity, DapperTestEntityTableDefinition.TableDefinition);

            foreach (var child in entity.Childs.AllItems)
            {
                CreateCommandDefinitionByState(
                    child,
                    DapperChildEntityTestTableDefinition.TableDefinition,
                    new Dictionary<string, IBaseEntity>() { { "ParentEntity", entity } });
            }

            return CommandDefinitions;
        }
    }
}