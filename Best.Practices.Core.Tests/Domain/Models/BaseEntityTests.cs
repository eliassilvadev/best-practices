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
            var minimalExpectedCreationDate = DateTime.UtcNow;

            var childClass = new ChildClassTest();

            childClass.Id.Should().NotBeEmpty();
            childClass.CreationDate.Should().BeAtLeast(minimalExpectedCreationDate.TimeOfDay);
            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsNew_SetStateAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsPersisted_StateShouldSettedAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsUnchanged_StateShouldRemainAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsUpdated_StateShouldBeSettedAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsUpdated();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsDeleted_StateShouldBeSettedAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUnchanged_StateIsPersistedDeleted_StateShouldBeSettedAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();
            childClass.SetStateAsPersisted();

            childClass.SetStateAsUnchanged();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsNew_StateShouldRemainAsNew()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUpdated();

            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsPersisted_StateShouldBeSettedAsUpdated()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            childClass.SetStateAsUpdated();

            childClass.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsDeleted_StateShouldRemainAsDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            childClass.SetStateAsUpdated();

            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsUpdated_StateIsUnchanged_StateShouldBeSettedAsUpdated()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            childClass.SetStateAsUpdated();

            childClass.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsNew_StateShouldBeSettedAsPersisted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            childClass.State.Should().Be(EntityState.Persisted);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsUnchanged_StateShouldRemainAsUnchanged()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            childClass.SetStateAsPersisted();

            childClass.State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public void SetStateAsPersisted_StateIsUpdated_StateShouldBeSettedAsPersisted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUpdated();

            childClass.SetStateAsPersisted();

            childClass.State.Should().Be(EntityState.Persisted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsPersistedDeleted_StateShouldBeSettedAsPersistedDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            childClass.SetStateAsPersisted();

            childClass.State.Should().Be(EntityState.PersistedDeleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsNew_StateShouldRemainAsNew()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.New);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsUpdated_StateShouldRemainAsDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsUpdated();

            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsPersisted_StateShouldRemainAsDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsPersisted();

            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsDeleted_StateShouldRemainAsDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();
            childClass.SetStateAsDeleted();

            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void SetStateAsDeleted_StateIsUnchanged_StateShouldBeSettedAsDeleted()
        {
            var childClass = new ChildClassTest();

            childClass.SetStateAsUnchanged();

            childClass.SetStateAsDeleted();

            childClass.State.Should().Be(EntityState.Deleted);
        }
    }
}