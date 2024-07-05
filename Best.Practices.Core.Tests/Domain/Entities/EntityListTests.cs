using Best.Practices.Core.Domain.Entities;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Tests.Application.Dtos.Builders;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Domain.Entities
{
    public class EntityListTests
    {
        [Fact]
        public void Constructor_WithNoParameters_ShouldInstantiate()
        {
            // Act
            var entityList = new EntityList<SampleEntity>();

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
            entityList.DeletedItems.Should().HaveCount(0);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithParentEntity_ShouldInstantiate()
        {
            // Arrange
            var entity = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>(entity);

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
            entityList.DeletedItems.Should().HaveCount(0);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().Be(entity);
        }

        [Fact]
        public void Constructor_WithParentEntityAndCapacity_ShouldInstantiate()
        {
            // Arrange
            var entity = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>(10, entity);

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
            entityList.DeletedItems.Should().HaveCount(0);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().Be(entity);
        }

        [Fact]
        public void Constructor_WithCapacity_ShouldInstantiate()
        {
            // Act
            var entityList = new EntityList<SampleEntity>(10);

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
            entityList.DeletedItems.Should().HaveCount(0);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEntityList_ShouldInstantiate()
        {
            // Arrange
            var newEntity = new SampleEntity();
            var newEntity2 = new SampleEntity();
            var deletedEntity = new SampleEntity();

            deletedEntity.SetStateAsUnchanged();
            deletedEntity.SetStateAsDeleted();

            var anotherEntityList = new EntityList<SampleEntity>
            {
                newEntity,
                newEntity2,
                deletedEntity
            };

            // Act
            var entityList = new EntityList<SampleEntity>(anotherEntityList);

            // Assert
            entityList.Should().HaveCount(2);
            entityList.Items.Should().HaveCount(2);
            entityList.AllItems.Should().HaveCount(3);
            entityList.DeletedItems.Should().HaveCount(1);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEntityListAndParent_ShouldInstantiate()
        {
            // Arrange
            var entity = new SampleEntity();

            var newEntity = new SampleEntity();
            var newEntity2 = new SampleEntity();
            var deletedEntity = new SampleEntity();

            deletedEntity.SetStateAsUnchanged();
            deletedEntity.SetStateAsDeleted();

            var anotherEntityList = new EntityList<SampleEntity>
            {
                newEntity,
                newEntity2,
                deletedEntity
            };

            // Act

            var entityList = new EntityList<SampleEntity>(anotherEntityList, entity);

            // Assert
            entityList.Should().HaveCount(2);
            entityList.Items.Should().HaveCount(2);
            entityList.AllItems.Should().HaveCount(3);
            entityList.DeletedItems.Should().HaveCount(1);
            entityList.IsFixedSize.Should().BeFalse();
            entityList.Parent.Should().Be(entity);
        }

        [Fact]
        public void IndexOf_EmptyList_ReturnsMinusOne()
        {
            // Arrange
            var entity = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>();

            // Assert
            entityList.IndexOf(entity).Should().Be(-1);
        }

        [Fact]
        public void IndexOf_ListWithTwoItems_ReturnsItemIndex()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2
            };

            // Assert
            entityList.IndexOf(entity2).Should().Be(1);
        }

        [Fact]
        public void IndexOf_GivenAInexistentItemToAListtWithTwoItems_ReturnsMinusOne()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();
            var entity3 = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2
            };

            // Assert
            entityList.IndexOf(entity3).Should().Be(-1);
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 1)]
        public void Insert_GivenAValidIndexToInserts_InsertTheItemInGivenPosition(int indexToInsert, int oldItemExpectedIndex, int newItemExpectedIndex)
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
            };

            // Act
            entityList.Insert(indexToInsert, entity2);

            // Assert
            entityList.IndexOf(entity).Should().Be(oldItemExpectedIndex);
            entityList.IndexOf(entity2).Should().Be(newItemExpectedIndex);
            entityList.Should().HaveCount(2);
        }

        [Fact]
        public void Insert_GivenAnInValidIndexToInsert_ThrowsException()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
            };

            Action action = () => entityList.Insert(2, entity2);

            // Act
            action.Should().Throw<ArgumentOutOfRangeException>();

            // Assert
            entityList.IndexOf(entity).Should().Be(0);
            entityList.IndexOf(entity2).Should().Be(-1);
            entityList.Should().HaveCount(1);
        }

        [Fact]
        public void RemoveAt_GivenAValidIndexToRemove_RemovesItem()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2,
            };

            // Act
            entityList.RemoveAt(1);

            // Assert
            entityList.IndexOf(entity).Should().Be(0);
            entityList.IndexOf(entity2).Should().Be(-1);
            entityList.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(2)]
        [InlineData(-1)]
        public void RemoveAt_GivenAnInValidIndexToRemove_ThrowsException(int indexToRemove)
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2,
            };

            // Act
            Action action = () => entityList.RemoveAt(indexToRemove);

            action.Should().Throw<ArgumentOutOfRangeException>();

            // Assert
            entityList.IndexOf(entity).Should().Be(0);
            entityList.IndexOf(entity2).Should().Be(1);
            entityList.Should().HaveCount(2);
        }

        [Fact]
        public void GetIndexedProperty_GivenAValidIndex_ReturnsItem()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2
            };

            // Act
            var searchedItem = entityList[0];

            // Assert
            searchedItem.Should().Be(entity);
        }

        [Fact]
        public void GetIndexedProperty_GivenAInValidIndex_ReturnsItem()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
                entity2
            };

            // Act
            Func<SampleEntity> function = () => entityList[2];

            // Arrange
            function.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void SetIndexedProperty_GivenAInValidIndex_ReturnsItem()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity
            };

            // Act
            entityList[0] = entity2;

            // Assert
            entityList[0].Should().Be(entity2);
            entityList.First().Should().Be(entity2);
            entityList.Should().HaveCount(1);
        }

        [Fact]
        public void Add_GivenAInstantiatedItem_AddsTheItem()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>();

            // Act
            entityList.Add(entity);

            // Assert
            entityList.First().Should().Be(entity);
            entityList.Should().HaveCount(1);
        }

        [Fact]
        public void Add_ListWithParentAndGivenAInstantiatedItem_AddsTheItemAndSetParetStateToUpdated()
        {
            // Arrange
            var parentEntity = new SampleEntity();

            parentEntity.SetStateAsUnchanged();

            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>(parentEntity);

            // Act
            entityList.Add(entity);

            // Assert
            entityList.First().Should().Be(entity);
            entityList.Should().HaveCount(1);
            parentEntity.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void AddRange_GivenAnEmptyList_AddsNoItemsToList()
        {
            // Arrange
            var emptyList = new EntityList<SampleEntity>();

            var entityList = new EntityList<SampleEntity>();

            // Act
            entityList.AddRange(emptyList);

            // Assert         
            entityList.Should().HaveCount(0);
        }

        [Fact]
        public void AddRange_GivenAListWithItems_AddsListItemsToList()
        {
            // Arrange
            var entity = new SampleEntity();

            var listWithItems = new EntityList<SampleEntity>()
            {
                entity
            };

            var entityList = new EntityList<SampleEntity>();

            // Act
            entityList.AddRange(listWithItems);

            // Assert
            entityList.First().Should().Be(entity);
            entityList.Should().HaveCount(listWithItems.Count());
        }

        [Fact]
        public void Clear_ListHasItems_ShouldRemoveAllItems()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity
            };

            // Act
            entityList.Clear();

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
        }

        [Fact]
        public void Clear_ListHasDeletedItems_ShouldRemoveAllItems()
        {
            // Arrange
            var newEntity = new SampleEntity();
            var deletedEntity = new SampleEntity();

            deletedEntity.SetStateAsUnchanged();
            deletedEntity.SetStateAsDeleted();

            var entityList = new EntityList<SampleEntity>
            {
                newEntity,
                deletedEntity
            };

            // Act
            entityList.Clear();

            // Assert
            entityList.Should().HaveCount(0);
            entityList.Items.Should().HaveCount(0);
            entityList.AllItems.Should().HaveCount(0);
            entityList.DeletedItems.Should().HaveCount(0);
        }

        [Fact]
        public void Contains_ListHasItemWithSameInstance_ReturnsTrue()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity
            };

            // Act
            var constainsResult = entityList.Contains(entity);

            // Assert
            constainsResult.Should().BeTrue();
        }

        [Fact]
        public void Contains_GivenAItemThatDoesNotExists_ReturnsFalse()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>();

            // Act
            var constainsResult = entityList.Contains(entity);

            // Assert
            constainsResult.Should().BeFalse();
        }

        [Fact]
        public void Contains_GivenANullItem_ReturnsFalse()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Act
            var constainsResult = entityList.Contains(null);

            // Assert
            constainsResult.Should().BeFalse();
        }

        [Fact]
        public void CopyTo_GivenAValidIndex_ShouldCopyItemToArray()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            var entityArray = new SampleEntity[1];

            // Act
            entityList.CopyTo(entityArray, 0);

            // Assert
            entityArray[0].Should().Be(entity);
            entityArray.Should().HaveCount(1);
        }

        [Fact]
        public void CopyTo_GivenAArrayAndValidIndex_ShouldCopyItemToArray()
        {
            // Arrange
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity,
                entity2
            };

            var entityArray = new SampleEntity[3];

            // Act
            entityList.CopyTo(entityArray as Array, 1);

            // Assert
            entityArray[0].Should().BeNull();
            entityArray.Should().Contain(entity);
            entityArray.Should().Contain(entity2);
            entityArray.Should().HaveCount(3);
        }

        [Fact]
        public void CopyTo_GivenAInValidIndex_ThrowsArgumentException()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            var entityArray = new SampleEntity[0];

            // Act
            Action action = () => entityList.CopyTo(entityArray, 10);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CountProperty_AddAnEntity_ReturnsEntityCount()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var itemCount = entityList.Count;

            // Assert
            itemCount.Should().Be(1);
        }

        [Fact]
        public void CountProperty_NoEntitiesAded_ReturnsEntityCountAsZero()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Assert
            var itemCount = entityList.Count;

            // Act
            itemCount.Should().Be(0);
        }

        [Fact]
        public void Count_AddAnEntity_ReturnsEntityCount()
        {
            // Arrange
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var itemCount = entityList.Count();

            // Assert
            itemCount.Should().Be(1);
        }

        [Fact]
        public void Count_NoEntitiesAded_ReturnsEntityCountAsZero()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Assert
            var itemCount = entityList.Count();

            // Act
            itemCount.Should().Be(0);
        }

        [Fact]
        public void IsReadOnly_Always_ReturnsFalse()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Assert
            entityList.IsReadOnly.Should().BeFalse();
        }

        [Fact]
        public void Remove_GivenAnExistentEntity_RemovesEntityFromList()
        {
            // Arrange
            var entity = new SampleEntity();

            entity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var itemRemoved = entityList.Remove(entity);

            // Assert
            itemRemoved.Should().BeTrue();
            entityList.DeletedItems.Should().Contain(entity);
            entityList.Should().HaveCount(0);
        }

        [Fact]
        public void Remove_GivenAnInexistentEntity_RemovesEntityFromList()
        {
            // Arrange
            var entity = new SampleEntity();
            var inExistentEntity = new SampleEntity();

            entity.SetStateAsUnchanged();
            inExistentEntity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var itemRemoved = entityList.Remove(inExistentEntity);

            // Assert          
            itemRemoved.Should().BeFalse();
            entityList.DeletedItems.Should().NotContain(inExistentEntity);
            entityList.Should().HaveCount(1);
        }

        [Fact]
        public void Remove_GivenAnExistentEntityToAListWithParent_RemovesEntityFromListAndSetEntityStateToUpdated()
        {
            // Arrange
            var entity = new SampleEntity();
            var parentEntity = new SampleEntity();

            entity.SetStateAsUnchanged();
            parentEntity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>(parentEntity)
            {
                entity
            };

            // Act
            var itemRemoved = entityList.Remove(entity);

            // Assert
            itemRemoved.Should().BeTrue();
            entityList.Should().HaveCount(0);
            entityList.DeletedItems.Should().Contain(entity);
            parentEntity.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void RemoveAll_GivenAnPredicateWithMatch_RemovesAllEntitiesThatMatchesFromList()
        {
            // Arrange
            var entity = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove")
                .Build();

            entity.SetStateAsUnchanged();

            var entity2 = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove2")
                .Build();

            entity.SetStateAsUnchanged();
            entity2.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>()
            {
                entity,
                entity2
            };

            // Act
            var itemRemoved = entityList.RemoveAll(x => x.SampleName == "SampleNameToRemove");

            // Assert
            itemRemoved.Should().Be(1);
            entityList.Should().NotContain(entity);
            entityList.Should().Contain(entity2);
            entityList.DeletedItems.Should().Contain(entity);
            entityList.DeletedItems.Should().NotContain(entity2);
            entityList.Should().HaveCount(1);
            entityList.DeletedItems.Should().HaveCount(1);
            entity.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void RemoveAll_GivenAnPredicateThatDoesNotMatch_ShouldNotRemoveAnyItemsFromList()
        {
            // Arrange
            var entity = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove")
                .Build();

            var entity2 = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove2")
                .Build();

            entity.SetStateAsUnchanged();
            entity2.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>()
            {
                entity,
                entity2
            };

            // Act
            var itemRemoved = entityList.RemoveAll(x => x.SampleName == "DoesNotMatch");

            // Assert
            itemRemoved.Should().Be(0);
            entityList.DeletedItems.Should().HaveCount(0);
            entityList.Should().HaveCount(2);
        }

        [Fact]
        public void RemoveAll_GivenAnExistentEntityToAListWithParent_RemovesEntityFromListAndSetEntityStateToUpdated()
        {
            // Arrange
            var entity = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove")
                .Build();

            var entity2 = new SampleEntityBuilder()
                .WithSampleName("SampleNameToRemove2")
                .Build();

            entity.SetStateAsUnchanged();
            entity2.SetStateAsUnchanged();

            var parentEntity = new SampleEntity();

            entity.SetStateAsUnchanged();
            parentEntity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>(parentEntity)
            {
                entity,
                entity2
            };

            // Act
            var itemRemoved = entityList.RemoveAll(x => x.SampleName == "SampleNameToRemove");

            // Assert
            itemRemoved.Should().Be(1);
            entityList.Should().HaveCount(1);
            entityList.Should().NotContain(entity);
            entityList.DeletedItems.Should().Contain(entity);
            entity.State.Should().Be(EntityState.Deleted);
            parentEntity.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void IsSynchronized_Always_ReturnsFalse()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Assert
            entityList.IsSynchronized.Should().BeFalse();
        }

        [Fact]
        public void SyncRoot_Always_ReturnsTheList()
        {
            // Arrange
            var entityList = new EntityList<SampleEntity>();

            // Assert
            entityList.SyncRoot.Should().Be(entityList);
        }

        [Fact]
        public void Add_GivenAnEntityAsObject_AddsEntityToList()
        {
            // Arrange
            object entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>();

            // Act
            var index = entityList.Add(entity);

            // Assert
            entityList.Should().HaveCount(1);
            index.Should().Be(0);
        }

        [Fact]
        public void Add_HasParentAndGivenAnEntityAsObject_AddsEntityToList()
        {
            // Arrange
            var parentEntity = new SampleEntity();
            object entity = new SampleEntity();

            parentEntity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>(parentEntity);

            // Act
            var index = entityList.Add(entity);

            // Assert
            entityList.Should().HaveCount(1);
            index.Should().Be(0);
            parentEntity.State.Should().Be(EntityState.Updated);
        }

        [Fact]
        public void Contains_GivenAnEntityThatExists_ReturnsTrue()
        {
            // Arrange          
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var contains = entityList.Contains(entity);

            // Assert
            contains.Should().BeTrue();
        }

        [Fact]
        public void Contains_GivenAnEntityThatDoesNotExists_ReturnsFalse()
        {
            // Arrange          
            var entity = new SampleEntity();
            var entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var contains = entityList.Contains(entity2);

            // Assert
            contains.Should().BeFalse();
        }

        [Fact]
        public void Contains_GivenAnNullEntityThatDoesNotExists_ReturnsFalse()
        {
            // Arrange          
            var entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var contains = entityList.Contains(null);

            // Assert
            contains.Should().BeFalse();
        }

        [Fact]
        public void Contains_GivenAnEntityAsObjectThatExists_ReturnsTrue()
        {
            // Arrange          
            object entity = new SampleEntity();

            var entityList = new EntityList<SampleEntity>()
            {
                entity
            };

            // Act
            var contains = entityList.Contains(entity);

            // Assert
            contains.Should().BeTrue();
        }

        [Fact]
        public void Contains_GivenAnEntityAsObjectThatExists_ReturnsIndex()
        {
            // Arrange
            object entity = new SampleEntity();

            // Act
            var entityList = new EntityList<SampleEntity>()
            {
                entity as SampleEntity
            };

            // Assert
            entityList.IndexOf(entity).Should().Be(0);
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 1)]
        public void Insert_GivenAValidIndexToInsertsAndAnEntityAsObject_InsertTheItemInGivenPosition(int indexToInsert, int oldItemExpectedIndex, int newItemExpectedIndex)
        {
            // Arrange
            var entity = new SampleEntity();
            object entity2 = new SampleEntity();

            var entityList = new EntityList<SampleEntity>
            {
                entity,
            };

            // Act
            entityList.Insert(indexToInsert, entity2);

            // Assert
            entityList.IndexOf(entity).Should().Be(oldItemExpectedIndex);
            entityList.IndexOf(entity2).Should().Be(newItemExpectedIndex);
            entityList.Should().HaveCount(2);
        }

        [Fact]
        public void Remove_GivenAnExistentEntityAsObject_RemovesEntityFromList()
        {
            // Arrange
            object entityAsObject = new SampleEntity();

            var entity = ((SampleEntity)entityAsObject);

            entity.SetStateAsUnchanged();

            var entityList = new EntityList<SampleEntity>()
            {
                entityAsObject
            };

            // Act
            entityList.Remove(entityAsObject);

            // Assert          
            entityList.DeletedItems.Should().Contain(entityAsObject as SampleEntity);
            entityList.Should().HaveCount(0);
            entity.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void Clone_GivenAnListWithItems_ReturnsANewListWithClonedItemsExceptDeletedItems()
        {
            // Arrange
            var entity1 = new SampleEntity();
            var entity2 = new SampleEntity();
            var entity3 = new SampleEntity();
            var entity4 = new SampleEntity();
            var entity5 = new SampleEntity();

            entity1.SetStateAsUnchanged();
            entity2.SetStateAsUnchanged();
            entity3.SetStateAsUnchanged();
            entity4.SetStateAsUnchanged();
            entity4.SetStateAsUpdated();
            entity5.SetStateAsUnchanged();
            entity5.SetStateAsDeleted();

            var entityList = new EntityList<SampleEntity>()
            {
                entity1,
                entity2,
                entity3,
                entity4,
                entity5
            };

            // Act
            var cloneList = entityList.Clone();

            // Assert          
            cloneList.Items.Should().HaveCount(4);
            cloneList.DeletedItems.Should().HaveCount(0);
            cloneList.Should().NotContain(entity1);
            cloneList.Should().NotContain(entity2);
            cloneList.Should().NotContain(entity3);
            cloneList.Should().NotContain(entity4);
            cloneList.Should().NotContain(entity5);

            cloneList[0].Id.Should().NotBe(entity1.Id);
            cloneList[0].State.Should().Be(EntityState.New);

            cloneList[1].Id.Should().NotBe(entity2.Id);
            cloneList[1].State.Should().Be(EntityState.New);

            cloneList[2].Id.Should().NotBe(entity3.Id);
            cloneList[2].State.Should().Be(EntityState.New);

            cloneList[3].Id.Should().NotBe(entity4.Id);
            cloneList[3].State.Should().Be(EntityState.New);
        }
    }
}