using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Exceptions;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using System.Data;
using Xunit;
using static Dapper.SqlMapper;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.Commands
{
    public class UpdateDapperTestEntityCommandTests
    {
        private readonly UpdateDapperTestEntityCommand _command;
        private readonly Mock<IDbConnection> _connection;
        private readonly Mock<DapperTestEntity> _entity;
        private readonly Mock<DapperTestEntity2> _entity2;
        private readonly Mock<DapperChildEntityTest> _childEntity;

        public UpdateDapperTestEntityCommandTests()
        {
            _connection = new Mock<IDbConnection>();
            _entity = new Mock<DapperTestEntity>();
            _entity2 = new Mock<DapperTestEntity2>();
            _childEntity = new Mock<DapperChildEntityTest>();

            _command = new UpdateDapperTestEntityCommand(_connection.Object, _entity.Object);
        }

        [Fact]
        public void GetCommandDefinitions_EntityIsUpdated_ReturnsUpdateSqlScript()
        {
            const string expectedUpdateSql = "Update EntityTestTable Set\nCode = @Code,\nName = @Name\nWhere\nId = @Id;";

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Code),"000001" },
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _entity.Setup(x => x.Childs).Returns([]);

            var commandDefinitions = _command.GetCommandDefinitions(_entity.Object);

            commandDefinitions.Should().HaveCount(1);
            commandDefinitions[0].CommandText.Should().BeEquivalentTo(expectedUpdateSql);
        }

        [Fact]
        public void GetCommandDefinitions_EntityHasOnlyOneUpdatedField_ReturnsUpdateSqlScript()
        {
            const string expectedUpdateSql = "Update EntityTestTable Set\nName = @Name\nWhere\nId = @Id;";

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _entity.Setup(x => x.Childs).Returns([]);

            var commandDefinitions = _command.GetCommandDefinitions(_entity.Object);

            commandDefinitions.Should().HaveCount(1);
            commandDefinitions[0].CommandText.Should().BeEquivalentTo(expectedUpdateSql);
        }

        [Fact]
        public void GetCommandDefinitions_EntityHasAUpdatedChild_ReturnCommandsForAgregatedRootAndChild()
        {
            const string expectedAgregatedRootUpdateSql = "Update EntityTestTable Set\nName = @Name\nWhere\nId = @Id;";
            const string expectedChildUpdateSql = "Update ChildEntityTestTable Set\nNumber = @Number\nWhere\nId = @Id;";

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            var childEntityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperChildEntityTest.Number),"000002" }
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _childEntity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetPropertiesToPersist()).Returns(childEntityUpdatedProperties);
            _entity.Setup(x => x.Childs).Returns([_childEntity.Object]);

            var commandDefinitions = _command.GetCommandDefinitions(_entity.Object);

            commandDefinitions.Should().HaveCount(2);
            commandDefinitions[0].CommandText.Should().BeEquivalentTo(expectedAgregatedRootUpdateSql);
            commandDefinitions[1].CommandText.Should().BeEquivalentTo(expectedChildUpdateSql);
        }

        [Fact]
        public void GetCommandDefinitions_EntityHasAnNewChild_ReturnCommandsForAgregatedRootAndChild()
        {
            const string expectedAgregatedRootUpdateSql = "Update EntityTestTable Set\nName = @Name\nWhere\nId = @Id;";
            const string expectedChildInsertSql = "Insert Into ChildEntityTestTable(\nNumber,\nDescription)\nValues(\n@Number,\n@Description);";

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            var childEntityInsertableProperties = new Dictionary<string, object>()
            {
                { nameof(DapperChildEntityTest.Number),"000002" },
                { nameof(DapperChildEntityTest.Description), null }
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _childEntity.Setup(x => x.State).Returns(EntityState.New);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetPropertiesToPersist()).Returns(childEntityInsertableProperties);
            _entity.Setup(x => x.Childs).Returns([_childEntity.Object]);

            var d = DapperChildEntityTestTableDefinition.TableDefinition;
            var d2 = DapperTestEntityTableDefinition.TableDefinition;

            var commandDefinitions = _command.GetCommandDefinitions(_entity.Object);

            commandDefinitions.Should().HaveCount(2);
            commandDefinitions[0].CommandText.Should().BeEquivalentTo(expectedAgregatedRootUpdateSql);
            commandDefinitions[1].CommandText.Should().BeEquivalentTo(expectedChildInsertSql);
        }

        [Fact]
        public void UpdateCommandByEntityUpdatedPropertiesWithCriteria_Always_ReturnsUpdateCommandDefinition()
        {
            const string expectedUpdateSql = "Update EntityTestTable Set\nName = @Name,\nCode = @Code\nWhere\nName = @Name\nAnd Code is null;";

            var updateCriteria = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name" },
                { nameof(DapperTestEntity.Code), null }
            };

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
                { nameof(DapperTestEntity.Code), null }
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);

            var commandDefinition = _command.UpdateCommandByEntityUpdatedPropertiesWithCriteria(
                _entity.Object,
                updateCriteria,
                DapperTestEntityTableDefinition.TableDefinition);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedUpdateSql);
        }

        [Fact]
        public void GetCommandDefinitions_EntityHasADeletedChild_ReturnCommandsForAgregatedRootAndChild()
        {
            const string expectedAgregatedRootUpdateSql = "Update EntityTestTable Set\nName = @Name\nWhere\nId = @Id;";
            const string expectedChildDeleteSql = "Delete From\nChildEntityTestTable\nWhere\nId = @Id;";

            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _childEntity.Setup(x => x.State).Returns(EntityState.Deleted);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetPropertiesToPersist()).Returns([]);
            _entity.Setup(x => x.Childs).Returns([_childEntity.Object]);

            var commandDefinitions = _command.GetCommandDefinitions(_entity.Object);

            commandDefinitions.Should().HaveCount(2);
            commandDefinitions[0].CommandText.Should().BeEquivalentTo(expectedAgregatedRootUpdateSql);
            commandDefinitions[1].CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void DeleteCommandWithEntityId_Always_ReturnsDeleteCommandDefinition()
        {
            const string expectedChildDeleteSql = "Delete From\nChildEntityTestTable\nWhere\nId = @Id;";

            _entity.Setup(x => x.State).Returns(EntityState.Deleted);
            _entity.Setup(x => x.Childs).Returns([]);

            var commandDefinition = _command.DeleteCommandWithEntityAndIdCriteria(
                _entity.Object,
                DapperChildEntityTestTableDefinition.TableDefinition);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void DeleteCommandWithCriteria_Always_ReturnsDeleteCommandDefinition()
        {
            const string expectedChildDeleteSql = "Delete From\nChildEntityTestTable\nWhere\nNumber = @Number\nAnd Description is null;";

            var deleteCriteria = new Dictionary<string, object>()
            {
                { nameof(DapperChildEntityTest.Number),"Number 001" },
                { nameof(DapperChildEntityTest.Description), null }
            };

            _childEntity.Setup(x => x.GetPropertiesToPersist()).Returns([]);

            var commandDefinition = _command.DeleteCommandWithCriteria(
                deleteCriteria,
                DapperChildEntityTestTableDefinition.TableDefinition);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void DeleteCommandWithCriteria_WhenHasAEntityInAProperty_ReturnsDeleteCommandDefinition()
        {
            var entityId = Guid.NewGuid();
            const string expectedChildDeleteSql = "Delete From\nEntityTestTable2\nWhere\nChildEntity_Id = @ChildEntity_Id;";

            _entity2.Setup(x => x.GetPropertiesToPersist()).Returns([]);

            _entity2.Object.ChildEntity = new DapperTestEntity();

            var deleteCriteria = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity2.ChildEntity), new DapperTestEntity() }
            };

            var commandDefinition = _command.DeleteCommandWithCriteria(
                deleteCriteria,
                DapperTestEntity2TableDefinition.TableDefinition);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void DeleteCommandWithCriteria_WhenHasAEntityInAPropertyAndColumnDefinitionHasSubProperty_ReturnsDeleteCommandDefinition()
        {
            var entityId = Guid.NewGuid();
            const string expectedChildDeleteSql = "Delete From\nEntityTestTable2\nWhere\nChildEntity2_Id = @ChildEntity2_Id;";

            _entity2.Setup(x => x.GetPropertiesToPersist()).Returns([]);

            _entity2.Object.ChildEntity = new DapperTestEntity();

            var deleteCriteria = new Dictionary<string, object>()
            {
                { "ChildEntity2.Id", new DapperTestEntity() }
            };

            var commandDefinition = _command.DeleteCommandWithCriteria(
                deleteCriteria,
                DapperTestEntity2TableDefinition.TableDefinition);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void Execute_ExecuteWithSuccess_ReturnsTrue()
        {
            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Code),"000001" },
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Returns(entityUpdatedProperties);
            _entity.Setup(x => x.Childs).Returns([]);

            _connection.SetupDapper(c => c.Execute(It.IsAny<CommandDefinition>())).Returns(0);

            var result = _command.Execute();

            result.Should().BeTrue();
        }

        [Fact]
        public void Execute_ConnectionThrowsException_ReturnsFalse()
        {
            var entityUpdatedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Code),"000001" },
                { nameof(DapperTestEntity.Name),"Name Test" },
            };

            _entity.Setup(x => x.State).Returns(EntityState.Updated);
            _entity.Setup(x => x.GetPropertiesToPersist()).Throws(new Exception("Error Test"));
            _entity.Setup(x => x.Childs).Returns([]);

            Action execute = () => _command.Execute();

            execute.Should().Throw<CommandExecutionException>().WithMessage(CommonConstants.ErrorMessages.DefaultErrorMessage + CommonConstants.StringEnter + "Error Test");
        }
    }
}