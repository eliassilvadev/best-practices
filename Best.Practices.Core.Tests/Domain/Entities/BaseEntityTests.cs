using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Entities;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Best.Practices.Core.Tests.Domain.Entities
{
    public class BaseEntityTests
    {
        [ExcludeFromCodeCoverage]
        private class ChildClassTest : BaseEntity
        {
            public string SampleName { get; set; }
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

        [Fact]
        public void EntityClone_Always_ShouldReturnANewEntity()
        {
            //Arrange
            var childClass = new ChildClassTest()
            {
                SampleName = "Name Test"
            };

            childClass.SetStateAsUnchanged();

            //Act
            var cloneEntity = (ChildClassTest)childClass.EntityClone();

            //Assert
            cloneEntity.Id.Should().NotBe(childClass.Id);
            cloneEntity.State.Should().Be(EntityState.New);
            cloneEntity.SampleName.Should().Be(childClass.SampleName);
        }

        [Fact]
        public async Task EntityClone_GivenAnEntitySomeHierarchy_ShouldReturnANewEntity()
        {
            //Arrange
            var agregatedRoot = new AgregatedRoot()
            {
                SampleName = "AgregatedRoot"
            };

            await Task.Delay(TimeSpan.FromSeconds(1));

            var childClassLevel2 = new ChildClassLevel2()
            {
                SampleName = "ChildClassLevel2"
            };

            await Task.Delay(TimeSpan.FromSeconds(1));

            var childClassLevel3 = new ChildClassLevel3()
            {
                SampleName = "ChildClassLevel3"
            };

            await Task.Delay(TimeSpan.FromSeconds(1));

            var childClassLevel1 = new AgregatedRoot()
            {
                SampleName = "ChildClassLevel3|Level1"
            };

            await Task.Delay(TimeSpan.FromSeconds(1));

            agregatedRoot.ChildClassLevel2 = childClassLevel2;
            childClassLevel2.ChildClassLevel3 = childClassLevel3;
            childClassLevel3.AgreegatedRoot = childClassLevel1;

            //Act
            var cloneEntity = (AgregatedRoot)agregatedRoot.EntityClone();

            //Assert
            cloneEntity.Id.Should().NotBe(agregatedRoot.Id);
            cloneEntity.State.Should().Be(EntityState.New);
            cloneEntity.SampleName.Should().Be(agregatedRoot.SampleName);
            cloneEntity.CreationDate.Should().NotBe(agregatedRoot.CreationDate);

            cloneEntity.ChildClassLevel2.Id.Should().Be(agregatedRoot.ChildClassLevel2.Id);
            cloneEntity.ChildClassLevel2.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.SampleName);
            cloneEntity.ChildClassLevel2.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.CreationDate);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.Id.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.Id);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.SampleName);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.CreationDate);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.Id.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.Id);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.SampleName);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.CreationDate);
        }

        [Fact]
        public void EntityClone_GivenAnEntityWithCircularReference_ShouldReturnANewEntityWithCirclarReferenceIgnored()
        {
            //Arrange
            var agregatedRoot = new AgregatedRoot()
            {
                SampleName = "AgregatedRoot",
                ChildClassLevel2 = new ChildClassLevel2()
                {
                    SampleName = "ChildClassLevel2",
                    ChildClassLevel3 = new ChildClassLevel3()
                    {
                        SampleName = "ChildClassLevel3"
                    }
                }
            };

            agregatedRoot.SetStateAsUnchanged();
            agregatedRoot.SetStateAsDeleted();

            agregatedRoot.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot = agregatedRoot; // circular reference

            //Act
            var cloneEntity = (AgregatedRoot)agregatedRoot.EntityClone();

            //Assert
            cloneEntity.Id.Should().NotBe(agregatedRoot.Id);
            cloneEntity.State.Should().Be(EntityState.New);
            cloneEntity.SampleName.Should().Be(agregatedRoot.SampleName);

            cloneEntity.ChildClassLevel2.Id.Should().Be(agregatedRoot.ChildClassLevel2.Id);
            cloneEntity.ChildClassLevel2.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.SampleName);
            cloneEntity.ChildClassLevel2.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.CreationDate);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.Id.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.Id);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.SampleName);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.CreationDate);
            cloneEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.Should().BeNull();
        }

        [Fact]
        public void Clone_Always_ShouldReturnANewEntity()
        {
            //Arrange
            var childClass = new ChildClassTest()
            {
                SampleName = "Name Test"
            };

            childClass.SetStateAsUnchanged();

            //Act
            var cloneEntity = (ChildClassTest)childClass.Clone();

            //Assert
            cloneEntity.Id.Should().NotBe(childClass.Id);
            cloneEntity.State.Should().Be(EntityState.New);
            cloneEntity.SampleName.Should().Be(childClass.SampleName);
        }

        [Fact]
        public void GetPropertiesToPersist_StateIsUnchanged_ReturnsEmptyDictinary()
        {
            //Arrange
            var childClass = new ChildClassTest()
            {
                SampleName = "Name Test"
            };

            childClass.SetStateAsUnchanged();

            //Act
            var propertiesToPersist = childClass.GetPropertiesToPersist();

            //Assert           
            propertiesToPersist.Should().HaveCount(0);
        }

        [Fact]
        public void GetPropertiesToPersist_StateIsNew_ReturnsAlInsertableProperties()
        {
            //Arrange
            var childClass = new ChildClassTest()
            {
                SampleName = "Name Test"
            };

            var dictionary = new Dictionary<string, object>()
            {
                {"SampleName", childClass.SampleName  },
                {"Id", childClass.Id  },
                {"CreationDate", childClass.CreationDate  }
            };

            //Act
            var propertiesToPersist = childClass.GetPropertiesToPersist();

            //Assert
            propertiesToPersist.Should().BeEquivalentTo(dictionary);
        }

        [Fact]
        public void GetPropertiesToPersist_StateIsUpdated_ReturnsUpdatedProperties()
        {
            //Arrange
            var childClass = new ChildClassTest()
            {
                SampleName = "Name Test"
            };

            childClass.SetStateAsUnchanged();

            childClass.SampleName = "Updated Name";

            childClass.SetStateAsUpdated();

            //Act
            var propertiesToPersist = childClass.GetPropertiesToPersist();

            //Assert           
            propertiesToPersist.Should().ContainEquivalentOf(new KeyValuePair<string, object>("SampleName", "Updated Name"));
        }
    }
}