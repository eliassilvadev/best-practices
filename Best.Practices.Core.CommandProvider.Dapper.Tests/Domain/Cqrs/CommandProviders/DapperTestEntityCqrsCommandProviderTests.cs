using Moq;
using System.Data;
using Xunit;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.CommandProviders
{
    public class DapperTestEntityCqrsCommandProviderTests
    {
        private readonly Mock<IDbConnection> _connection;
        private readonly DapperTestEntityCqrsCommandProvider _commandProvider;

        public DapperTestEntityCqrsCommandProviderTests()
        {
            _connection = new Mock<IDbConnection>();

            _commandProvider = new DapperTestEntityCqrsCommandProvider(_connection.Object);
        }

        [Fact]
        public void GetCommandDefinitions_EntityIsUpdated_ReturnsUpdateSqlScript()
        {
        }
    }
}