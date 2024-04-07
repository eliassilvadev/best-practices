using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Domain.Models
{
    public class BaseEntityTests
    {
        private class ChildClassTest : BaseEntity
        {
        }

        [Fact]
        public void Constructor_Always_InitializateProperties()
        {
            //Arrange
            var minimalExpectedCreationDate = DateTime.UtcNow;

            //Act
            var childClass = new ChildClassTest();

            //Assert
            childClass.Id.Should().NotBeEmpty();
            childClass.CreationDate.Should().BeAtLeast(minimalExpectedCreationDate.TimeOfDay);
            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsNew_SetStateAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsPersisted_StateShouldSettedAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsUnchanged_StateShouldRemainAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsUpdated_StateShouldBeSettedAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsUpdated();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsDeleted_StateShouldBeSettedAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsPersistedDeleted_StateShouldBeSettedAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();
            childClass.SetStateAsPersisted();

            //Act
            childClass.SetStateAsUnchanged();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsNew_StateShouldRemainAsNew()
        {
            //Arrange
            var childClass = new ChildClassTest();

            //Act
            childClass.SetStateAsUpdated();

            //Assert
            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsPersisted_StateShouldBeSettedAsUpdated()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            //Act
            childClass.SetStateAsUpdated();

            //Assert
            childClass.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsDeleted_StateShouldRemainAsDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            //Act
            childClass.SetStateAsUpdated();

            //Assert
            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsUnchanged_StateShouldBeSettedAsUpdated()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            //Act
            childClass.SetStateAsUpdated();

            //Assert
            childClass.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsNew_StateShouldBeSettedAsPersisted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            //Act
            childClass.SetStateAsPersisted();

            //Assert
            childClass.State.Should().Be(EntityState.Persisted);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsUnchanged_StateShouldRemainAsUnchanged()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            //Act
            childClass.SetStateAsPersisted();

            //Assert
            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsUpdated_StateShouldBeSettedAsPersisted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUpdated();

            //Act
            childClass.SetStateAsPersisted();

            //Assert
            childClass.State.Should().Be(EntityState.Persisted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsPersistedDeleted_StateShouldBeSettedAsPersistedDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            //Act
            childClass.SetStateAsPersisted();

            //Assert
            childClass.State.Should().Be(EntityState.PersistedDeleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsNew_StateShouldRemainAsNew()
        {
            //Arrange
            var childClass = new ChildClassTest();

            //Act
            childClass.SetStateAsDeleted();

            //Assert
            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsUpdated_StateShouldRemainAsDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsUpdated();

            //Act
            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsPersisted_StateShouldRemainAsDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            //Act
            childClass.SetStateAsDeleted();

            //Assert
            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsDeleted_StateShouldRemainAsDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            //Act
            childClass.SetStateAsDeleted();

            //Assert
            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsUnchanged_StateShouldBeSettedAsDeleted()
        {
            //Arrange
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            //Act
            childClass.SetStateAsDeleted();

            //Assert
            childClass.State.Should().Be(EntityState.Deleted);
        }
    }
}