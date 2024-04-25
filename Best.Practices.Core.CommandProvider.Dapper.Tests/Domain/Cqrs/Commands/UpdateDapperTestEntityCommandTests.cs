using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions;
using Best.Practices.Core.Domain.Enumerators;
using FluentAssertions;
using Moq;
using System.Data;
using Xunit;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.Commands
{
    public class UpdateDapperTestEntityCommandTests
    {
        private readonly UpdateDapperTestEntityCommand _command;
        private readonly Mock<IDbConnection> _connection;
        private readonly Mock<DapperTestEntity> _entity;
        private readonly Mock<DapperChildEntityTest> _childEntity;

        public UpdateDapperTestEntityCommandTests()
        {
            _connection = new Mock<IDbConnection>();
            _entity = new Mock<DapperTestEntity>();
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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);
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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);
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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetUpdatedProperties()).Returns(childEntityUpdatedProperties);
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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetInsertableProperties()).Returns(childEntityInsertableProperties);
            _entity.Setup(x => x.Childs).Returns([_childEntity.Object]);

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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);

            var commandDefinition = _command.UpdateCommandByEntityUpdatedPropertiesWithCriteria(
                _entity.Object,
                updateCriteria,
                DapperTestEntityTableDefinition.TableColumnDefinitions,
                "EntityTestTable");

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
            _entity.Setup(x => x.GetUpdatedProperties()).Returns(entityUpdatedProperties);
            _childEntity.Setup(x => x.GetInsertableProperties()).Returns([]);
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
                DapperTestEntityTableDefinition.TableColumnDefinitions,
                "ChildEntityTestTable");

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }

        [Fact]
        public void DeleteCommandWithCriteria_Always_ReturnsDeleteCommandDefinition()
        {
            const string expectedChildDeleteSql = "Delete From\nChildEntityTestTable\nWhere\nName = @Name\nAnd Code is null;";

            var deleteCriteria = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity.Name),"Name Test" },
                { nameof(DapperTestEntity.Code), null }
            };

            _childEntity.Setup(x => x.GetInsertableProperties()).Returns([]);

            var commandDefinition = _command.DeleteCommandWithCriteria(
                deleteCriteria,
                DapperTestEntityTableDefinition.TableColumnDefinitions,
                "ChildEntityTestTable");

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }
    }
}