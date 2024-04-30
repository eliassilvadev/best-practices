using Best.Practices.Core.CommandProvider.Dapper.UnitOfWork;
using Moq;
using System.Data;
using Xunit;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.UnitOfWork
{
    public class DapperUnitOfWorkTests
    {
        private readonly Mock<IDbConnection> _connection;
        private readonly Mock<IDbTransaction> _transaction;
        private readonly DapperUnitOfWork _unitOfWork;
        public DapperUnitOfWorkTests()
        {
            _connection = new Mock<IDbConnection>();
            _transaction = new Mock<IDbTransaction>();

            _unitOfWork = new DapperUnitOfWork(_connection.Object);
        }

        [Fact]
        public void Dispose_Always_CallingGC()
        {
            _unitOfWork.Dispose();
        }

        [Fact]
        public async Task BeforeSave_ConnectionIsClosed_CallOpenMethod()
        {
            await _unitOfWork.BeforeSaveAsync();

            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            _connection.Verify(x => x.Open(), Times.Once);
        }

        [Fact]
        public async Task AfterSave_WhenParameterSuccessIsTrue_CallTransactionCommit()
        {
            //Arrange
            _connection.Setup(x => x.BeginTransaction()).Returns(_transaction.Object);
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterSave(true);

            //Assert
            _connection.Verify(x => x.Open(), Times.Once);
            _transaction.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task AfterSave_WhenParameterSuccessIsFalse_ShouldNotCallTransactionCommit()
        {
            //Arrange
            _connection.Setup(x => x.BeginTransaction()).Returns(_transaction.Object);
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterSave(false);

            //Assert
            _connection.Verify(x => x.Open(), Times.Once);
            _transaction.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task AfterRollBack_Always_ShouldCallTransactionRollback()
        {
            //Arrange
            _connection.Setup(x => x.BeginTransaction()).Returns(_transaction.Object);
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterRollBackAsync();

            //Assert
            _transaction.Verify(x => x.Rollback(), Times.Once);
        }
    }
}
