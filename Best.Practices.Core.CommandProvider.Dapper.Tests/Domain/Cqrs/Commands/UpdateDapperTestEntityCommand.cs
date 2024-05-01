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
            DapperTestEntity affectedEntity)
            : base(connection, affectedEntity)
        {

            _entityTableTypeMappings.Add(nameof(DapperTestEntity), DapperTestEntityTableDefinition.TableDefinition);
            _entityTableTypeMappings.Add(nameof(DapperChildEntityTest), DapperChildEntityTestTableDefinition.TableDefinition);
        }

        public override IList<CommandDefinition> CreateCommandDefinitions(DapperTestEntity entity)
        {
            base.CreateCommandDefinitions(entity);

            CreateCommandDefinitionByState(entity);

            foreach (var child in entity.Childs.AllItems)
            {
                CreateCommandDefinitionByState(
                    child,
                    new Dictionary<string, IBaseEntity>() { { "ParentEntity", entity } });
            }

            return CommandDefinitions;
        }
    }
}