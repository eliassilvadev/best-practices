using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Interceptors;
using Best.Practices.Core.Domain.Entities;
using Best.Practices.Core.Tests.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Domain.Interceptors
{
    public class EntityStateControlInterceptorLinfuTests
    {
        [Fact]
        public void CreateEntityWithStateControl_EntityWithOtherEntityParts_ShouldCreateAProxyEntity()
        {
            //Arrange
            var agregatedRoot = new AgregatedRoot()
            {
                SampleName = "AgregatedRoot",
                Items = new EntityList<ChildClassListItem>()
                {
                    new ChildClassListItem()
                    {
                        SampleName = "ChildClassListItem"
                    }
                },
                ChildClassLevel2 = new ChildClassLevel2()
                {
                    Items = new EntityList<ChildClassListItem2>()
                    {
                        new ChildClassListItem2()
                        {
                            ChildClassLevel3 = new ChildClassLevel3()
                            {
                                SampleName = "ChildClassLevel3"
                            },
                            SampleName = "ChildClassListItem2",
                            SampleSurname = "SampleSurName"
                        },
                        new ChildClassListItem2()
                        {
                            ChildClassLevel3 = new ChildClassLevel3()
                            {
                                SampleName = "ChildClassLevel3_Item2"
                            },
                            SampleName = "ChildClassListItem2_Item2",
                            SampleSurname = "SampleSurName"
                        }
                    },
                    SampleName = "ChildClassLevel2",
                    ChildClassLevel3 = new ChildClassLevel3()
                    {
                        SampleName = "ChildClassLevel3"
                    }
                }
            };

            agregatedRoot.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptorLinfu(agregatedRoot);

            var proxyEntity = interceptor.CreateEntityWihStateControl(agregatedRoot);

            //Act
            proxyEntity.ChildClassLevel2.Items[0].ChildClassLevel3.SampleName = "Test";

            //Assert
            proxyEntity.Id.Should().Be(agregatedRoot.Id);

            proxyEntity.ChildClassLevel2.Items[0].ChildClassLevel3.State.Should().Be(EntityState.Updated);
            proxyEntity.ChildClassLevel2.Items[0].State.Should().Be(EntityState.Updated);
            proxyEntity.ChildClassLevel2.State.Should().Be(EntityState.Updated);
            proxyEntity.State.Should().Be(EntityState.Updated);

            proxyEntity.ChildClassLevel2.ChildClassLevel3.State.Should().Be(EntityState.Unchanged);

            proxyEntity.SampleName.Should().Be(agregatedRoot.SampleName);
            proxyEntity.CreationDate.Should().Be(agregatedRoot.CreationDate);

            proxyEntity.ChildClassLevel2.Id.Should().Be(agregatedRoot.ChildClassLevel2.Id);
            proxyEntity.ChildClassLevel2.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.SampleName);
            proxyEntity.ChildClassLevel2.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.CreationDate);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.Id.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.Id);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.SampleName);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.CreationDate);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.Should().BeNull();
        }

        [Fact]
        public void CreateEntityWithStateControlaaaaaaaaaa_EntityWithOtherEntityParts_ShouldCreateAProxyEntity()
        {
            //Arrange
            var agregatedRoot = new AgregatedRoot()
            {
                SampleName = "AgregatedRoot",
                Items = new EntityList<ChildClassListItem>()
                {
                    new ChildClassListItem()
                    {
                        SampleName = "ChildClassListItem"
                    }
                },
                ChildClassLevel2 = new ChildClassLevel2()
                {
                    Items = new EntityList<ChildClassListItem2>()
                    {
                        new ChildClassListItem2()
                        {
                            ChildClassLevel3 = new ChildClassLevel3()
                            {
                                SampleName = "ChildClassLevel3"
                            },
                            SampleName = "ChildClassListItem2",
                            SampleSurname = "SampleSurName"
                        },
                        new ChildClassListItem2()
                        {
                            ChildClassLevel3 = new ChildClassLevel3()
                            {
                                SampleName = "ChildClassLevel3_Item2"
                            },
                            SampleName = "ChildClassListItem2_Item2",
                            SampleSurname = "SampleSurName"
                        }
                    },
                    SampleName = "ChildClassLevel2",
                    ChildClassLevel3 = new ChildClassLevel3()
                    {
                        SampleName = "ChildClassLevel3"
                    }
                }
            };

            agregatedRoot.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptorLinfu(agregatedRoot);

            var proxyEntity = interceptor.CreateEntityWihStateControl(agregatedRoot);

            //Act
            proxyEntity.ChildClassLevel2.Items[0].ChildClassLevel3.SetSampleName("Test");

            //Assert
            proxyEntity.Id.Should().Be(agregatedRoot.Id);

            proxyEntity.ChildClassLevel2.Items[0].ChildClassLevel3.State.Should().Be(EntityState.Updated);
            proxyEntity.ChildClassLevel2.Items[0].State.Should().Be(EntityState.Updated);
            proxyEntity.ChildClassLevel2.State.Should().Be(EntityState.Updated);
            proxyEntity.State.Should().Be(EntityState.Updated);

            proxyEntity.ChildClassLevel2.ChildClassLevel3.State.Should().Be(EntityState.Unchanged);

            proxyEntity.SampleName.Should().Be(agregatedRoot.SampleName);
            proxyEntity.CreationDate.Should().Be(agregatedRoot.CreationDate);

            proxyEntity.ChildClassLevel2.Id.Should().Be(agregatedRoot.ChildClassLevel2.Id);
            proxyEntity.ChildClassLevel2.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.SampleName);
            proxyEntity.ChildClassLevel2.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.CreationDate);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.Id.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.Id);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.SampleName.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.SampleName);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.CreationDate.Should().Be(agregatedRoot.ChildClassLevel2.ChildClassLevel3.CreationDate);
            proxyEntity.ChildClassLevel2.ChildClassLevel3.AgreegatedRoot.Should().BeNull();
        }

        [Fact]
        public void CreateEntityWithStateControlAndCallRemoveAllFromList_EntityWithOtherEntityParts_ShouldCreateAProxyEntity()
        {
            //Arrange

            var child = new ChildClassListItem()
            {
                SampleName = "ChildClassListItem"
            };

            var agregatedRoot = new AgregatedRoot()
            {
                Items = new EntityList<ChildClassListItem>()
                {
                    child
                }
            };

            agregatedRoot.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptorLinfu(agregatedRoot);

            var proxyEntity = interceptor.CreateEntityWihStateControl(agregatedRoot);

            //Act
            agregatedRoot.Items.RemoveAll(x => x.SampleName == "ChildClassListItem");

            //Assert
            child.State.Should().Be(EntityState.Deleted);
        }

        [Fact]
        public void CreateEntityWithStateControl_CallMethdAMillionTimes_ShouldFinishLessThan3Seconds()
        {
            //Arrange
            var agregatedRoot = new AgregatedRoot()
            {
                SampleName = "AgregatedRoot",
                Items = new EntityList<ChildClassListItem>()
                {
                    new ChildClassListItem()
                    {
                        SampleName = "ChildClassListItem"
                    }
                },
                ChildClassLevel2 = new ChildClassLevel2()
                {
                    Items = new EntityList<ChildClassListItem2>()
                    {
                        new ChildClassListItem2()
                        {
                            ChildClassLevel3 = new ChildClassLevel3()
                            {
                                SampleName = "ChildClassLevel3"
                            },
                            SampleName = "ChildClassListItem2",
                            SampleSurname = "SampleSurName"
                        }
                    },
                    SampleName = "ChildClassLevel2",
                    ChildClassLevel3 = new ChildClassLevel3()
                    {
                        SampleName = "ChildClassLevel3"
                    }
                }
            };

            agregatedRoot.SetStateAsUnchanged();

            var interceptor = new EntityStateControlInterceptorLinfu(agregatedRoot);

            var proxyEntity = interceptor.CreateEntityWihStateControl(agregatedRoot);

            //Act
            int maxIterations = 1000000;

            var timeBeforeExecution = DateTime.Now.TimeOfDay;

            for (int i = 0; i <= maxIterations; i++)
            {
                proxyEntity.ChildClassLevel2.Items[0].ChildClassLevel3.SampleName = "Test";
            }

            var timeAfterExecution = DateTime.Now.TimeOfDay;

            var elapsedTime = timeAfterExecution.Subtract(timeBeforeExecution);

            elapsedTime.Should().BeLessThanOrEqualTo(TimeSpan.FromSeconds(3));
        }
    }
}