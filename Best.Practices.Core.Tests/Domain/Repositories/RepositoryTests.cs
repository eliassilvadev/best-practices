using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Entities;
using Best.Practices.Core.Domain.Entities.Interfaces;
using Best.Practices.Core.Domain.Repositories;
using Best.Practices.Core.UnitOfWork.Interfaces;
using Moq;
using Xunit;

namespace Best.Practices.Core.Tests.Domain.Repositories
{
    public class RepositoryTests
    {
        public class ChildClassTest : BaseEntity
        {
        }

        public class ChildRepositoryTest(ICqrsCommandProvider<IBaseEntity> commandProvider) : Repository<IBaseEntity>(commandProvider)
        {
        }

        private readonly ChildRepositoryTest _repository;
        private readonly Mock<ICqrsCommandProvider<IBaseEntity>> _commandProvider;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IBaseEntity> _entity;

        public RepositoryTests()
        {
            _commandProvider = new Mock<ICqrsCommandProvider<IBaseEntity>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _entity = new Mock<IBaseEntity>();

            _repository = new ChildRepositoryTest(_commandProvider.Object);
        }

        [Fact]
        public async Task GetById_Always_StateSetAsUnChanged()
        {
            //Arrange
            _commandProvider.Setup(c => c.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(_entity.Object);

            //Act
            await _repository.GetById(It.IsAny<Guid>());

            //Assert
            _entity.Verify(e => e.SetStateAsUnchanged(), Times.Once);
        }

        [Fact]
        public async Task GetById_EntityDoesNotExists_ReturnsNull()
        {
            //Arrange
            _commandProvider.Setup(c => c.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(null as IBaseEntity);

            //Act
            await _repository.GetById(It.IsAny<Guid>());

            //Assert
            _entity.Verify(e => e.SetStateAsUnchanged(), Times.Never);
        }

        [Fact]
        public void Persist_StateIsNew_InvokeCreationOfAddCommand()
        {
            //Arrange
            _entity.Setup(e => e.State).Returns(EntityState.New);

            //Act
            _repository.Persist(_entity.Object, _unitOfWork.Object);

            //Assert
            _unitOfWork.Verify(c => c.AddComand(It.IsAny<IEntityCommand>()), Times.Once);
            _commandProvider.Verify(c => c.GetAddCommand(It.IsAny<IBaseEntity>()), Times.Once);
            _commandProvider.Verify(c => c.GetUpdateCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetDeleteCommand(It.IsAny<IBaseEntity>()), Times.Never);
        }

        [Fact]
        public void Persist_StateIsUnchanged_InvokeNoMethod()
        {
            //Arrange
            _entity.Setup(e => e.State).Returns(EntityState.Unchanged);

            //Act
            _repository.Persist(_entity.Object, _unitOfWork.Object);

            //Assert
            _unitOfWork.Verify(c => c.AddComand(It.IsAny<IEntityCommand>()), Times.Never);
            _commandProvider.Verify(c => c.GetAddCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetUpdateCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetDeleteCommand(It.IsAny<IBaseEntity>()), Times.Never);
        }

        [Fact]
        public void Persist_StateIsUpdated_InvokeCreationOfUpdateCommand()
        {
            //Arrange
            _entity.Setup(e => e.State).Returns(EntityState.Updated);

            //Act
            _repository.Persist(_entity.Object, _unitOfWork.Object);

            //Assert
            _unitOfWork.Verify(c => c.AddComand(It.IsAny<IEntityCommand>()), Times.Once);
            _commandProvider.Verify(c => c.GetAddCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetUpdateCommand(It.IsAny<IBaseEntity>()), Times.Once);
            _commandProvider.Verify(c => c.GetDeleteCommand(It.IsAny<IBaseEntity>()), Times.Never);
        }

        [Fact]
        public void Persist_StateIsDeleted_InvokeCreationOfDeletedCommand()
        {
            //Arrange
            _entity.Setup(e => e.State).Returns(EntityState.Deleted);

            //Act
            _repository.Persist(_entity.Object, _unitOfWork.Object);

            //Assert
            _unitOfWork.Verify(c => c.AddComand(It.IsAny<IEntityCommand>()), Times.Once);
            _commandProvider.Verify(c => c.GetAddCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetUpdateCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetDeleteCommand(It.IsAny<IBaseEntity>()), Times.Once);
        }

        [Fact]
        public void Persist_StateIsPersistedDeleted_InvokeCreationOfDeletedCommand()
        {
            //Arrange
            _entity.Setup(e => e.State).Returns(EntityState.PersistedDeleted);

            //Act
            _repository.Persist(_entity.Object, _unitOfWork.Object);

            //Assert
            _unitOfWork.Verify(c => c.AddComand(It.IsAny<IEntityCommand>()), Times.Once);
            _commandProvider.Verify(c => c.GetAddCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetUpdateCommand(It.IsAny<IBaseEntity>()), Times.Never);
            _commandProvider.Verify(c => c.GetDeleteCommand(It.IsAny<IBaseEntity>()), Times.Once);
        }
    }
}