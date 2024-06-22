using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Tests.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Extensions
{
    public class IBaseEntityEntensionTests
    {
        public IBaseEntityEntensionTests()
        {
        }

        [Fact]
        public void EntityClone_Always_CreateANewEntity()
        {
            var sampleEntity = new SampleEntityWithChilds()
            {
                SampleCode = "001",
                SampleName = "SampleEntityWithChildsName"
            };

            var child = new ChildClassListItem() { SampleName = "ChilNameTest" };

            sampleEntity.AddChild(child);

            sampleEntity.SetStateAsUnchanged();
            sampleEntity.SetStateAsUpdated();

            var entityClone = (SampleEntityWithChilds)sampleEntity.EntityClone();

            entityClone.Id.Should().NotBe(sampleEntity.Id);
            entityClone.CreationDate.Should().NotBe(sampleEntity.CreationDate);
            entityClone.State.Should().Be(EntityState.New);
            sampleEntity.State.Should().Be(EntityState.Updated);
            entityClone.SampleName.Should().Be(sampleEntity.SampleName);
            entityClone.Childs.Should().HaveCount(sampleEntity.Childs.Count);
            entityClone.Childs[0].Id.Should().Be(child.Id);
            entityClone.Childs[0].SampleName.Should().Be(child.SampleName);
        }

        [Fact]
        public void Copy_Always_CopyDataMaintainingSameEntityInstance()
        {
            var sampleEntity = new SampleEntityWithChilds()
            {
                SampleCode = "001",
                SampleName = "SampleEntityWithChildsName"
            };

            var child = new ChildClassListItem() { SampleName = "ChilNameTest" };

            sampleEntity.AddChild(child);

            var entityCopy = new SampleEntityWithChilds();
            entityCopy.SetStateAsUnchanged();
            entityCopy.SetStateAsUpdated();

            entityCopy.Copy(sampleEntity);

            var expectedId = entityCopy.Id;
            var expectedCreationDate = entityCopy.CreationDate;
            var expectedState = entityCopy.State;

            entityCopy.Id.Should().Be(expectedId);
            entityCopy.CreationDate.Should().Be(expectedCreationDate);
            entityCopy.State.Should().Be(expectedState);
            entityCopy.SampleCode.Should().Be(sampleEntity.SampleCode);
            entityCopy.SampleName.Should().Be(sampleEntity.SampleName);
        }
    }
}