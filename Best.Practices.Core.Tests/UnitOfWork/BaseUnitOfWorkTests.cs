using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Best.Practices.Core.Tests.UnitOfWork
{
    public class BaseUnitOfWorkTests
    {

        [ExcludeFromCodeCoverage]
        public class ChildClassTest : BaseEntity
        {
        }

        [ExcludeFromCodeCoverage]
        public class ChildUnitOfWorkTest : BaseUnitOfWork
        {
            public override void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        private readonly ChildUnitOfWorkTest _unitOfWork;
        private readonly Mock<IEntityCommand> _entityComand;

        public BaseUnitOfWorkTests()
        {
            _entityComand = new Mock<IEntityCommand>();
            _unitOfWork = new ChildUnitOfWorkTest();
        }

        [Fact]
        public void Constructor_Always_InstantiateCommandList()
        {
            //Act
            var unitOfWork = new ChildUnitOfWorkTest();

            //Assert
            unitOfWork.Commands.Should().NotBeNull();
            unitOfWork.Commands.Should().HaveCount(0);
        }

        [Fact]
        public void AddComand_Always_AddCommandToCommandList()
        {
            //Act
            _unitOfWork.AddComand(_entityComand.Object);

            //Assert
            _unitOfWork.Commands.Should().HaveCount(1);
            _unitOfWork.Commands.Should().Contain(_entityComand.Object);
        }

        [Fact]
        public void SaveChanges_ComandsExecutedWithSucess_ReturnsTrue()
        {
            //Arrange
            var entity = new ChildClassTest();

            _entityComand.Setup(c => c.AffectedEntity).Returns(entity);
            _entityComand.Setup(c => c.Execute()).Returns(true);

            //Act
            _unitOfWork.AddComand(_entityComand.Object);
            var commandsCount = _unitOfWork.Commands.Count();

            var expectedReturn = _unitOfWork.SaveChanges();

            //Assert
            _unitOfWork.Commands.Should().HaveCount(0);
            _entityComand.Verify(c => c.Execute(), Times.Exactly(commandsCount));
            expectedReturn.Should().BeTrue();
        }

        [Fact]
        public void SaveChanges_ComandsExecutedWithNoSucess_ReturnsFalse()
        {
            //Arrange
            var entity = new ChildClassTest();

            _entityComand.Setup(c => c.AffectedEntity).Returns(entity);
            _entityComand.Setup(c => c.Execute()).Returns(false);

            //Act
            _unitOfWork.AddComand(_entityComand.Object);
            var commandsCount = _unitOfWork.Commands.Count();

            var expectedReturn = _unitOfWork.SaveChanges();

            //Assert
            _unitOfWork.Commands.Should().HaveCount(0);
            _entityComand.Verify(c => c.Execute(), Times.Exactly(commandsCount));
            expectedReturn.Should().BeFalse();
        }

        [Fact]
        public void SaveChanges_CommandThrowsException_ReturnsFalse()
        {
            //Arrange
            _entityComand.Setup(c => c.Execute()).Throws(new CommandExecutionException("Error Tesst"));
            _unitOfWork.AddComand(_entityComand.Object);
            var commandsCount = _unitOfWork.Commands.Count();

            //Act
            var expectedReturn = _unitOfWork.SaveChanges();

            //Assert
            _unitOfWork.Commands.Should().HaveCount(0);
            _entityComand.Verify(c => c.Execute(), Times.Exactly(commandsCount));
            expectedReturn.Should().BeFalse();
        }

        [Fact]
        public void SetEntitiesPersistedState_Always_AddCommandToCommandList()
        {
            // Arrange
            var entity = new ChildClassTest();

            _entityComand.Setup(c => c.AffectedEntity).Returns(entity);
            //Act
            _unitOfWork.AddComand(_entityComand.Object);

            _unitOfWork.SetEntitiesPersistedState();

            //Assert
            _unitOfWork.Commands.Should().HaveCount(1);
            entity.State.Should().Be(EntityState.Persisted);
        }

        [Fact]
        public void Rollback_Always_ClearCommandList()
        {
            // Arrange
            var entity = new ChildClassTest();

            _entityComand.Setup(c => c.AffectedEntity).Returns(entity);
            //Act
            _unitOfWork.AddComand(_entityComand.Object);

            _unitOfWork.Rollback();

            //Assert
            _unitOfWork.Commands.Should().HaveCount(0);
            entity.State.Should().Be(EntityState.New);
        }
    }
}