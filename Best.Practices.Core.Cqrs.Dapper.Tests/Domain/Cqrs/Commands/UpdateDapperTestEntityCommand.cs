using Best.Practices.Core.Cqrs.Dapper.EntityCommands;
using Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities;
using Best.Practices.Core.Cqrs.Dapper.Tests.TableDefinitions;
using Dapper;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Cqrs.CommandProviders.Commands
{
    public class UpdateDapperTestEntityCommand : DapperCommand<DapperTestEntity>
    {
        public UpdateDapperTestEntityCommand(
            IDbConnection connection,
            DapperTestEntity affectedEntity)
            : base(connection, affectedEntity)
        {

            AddTypeMapping(nameof(DapperTestEntity), DapperTestEntityTableDefinition.TableDefinition);

            AddTypeMapping(nameof(DapperChildEntityTest), DapperChildEntityTestTableDefinition.TableDefinition)
                .WithParentEntity("ParentEntity", affectedEntity);
        }

        public override IList<CommandDefinition> CreateCommandDefinitions(DapperTestEntity entity)
        {
            base.CreateCommandDefinitions(entity);

            CreateCommandDefinitionByState(entity);

            CreateCommandDefinitionByState(entity.Childs.AllItems);

            return CommandDefinitions;
        }
    }
}