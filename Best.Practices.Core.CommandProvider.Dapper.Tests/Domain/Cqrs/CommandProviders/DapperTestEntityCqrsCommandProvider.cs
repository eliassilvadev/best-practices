using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.CommandProviders.Interfaces;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.Commands;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.Domain.Cqrs;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.CommandProviders
{
    [ExcludeFromCodeCoverage]
    public class DapperTestEntityCqrsCommandProvider : DapperCqrsCommandProvider<DapperTestEntity>, IDapperTestEntityCqrsCommandProvider
    {
        public DapperTestEntityCqrsCommandProvider(IDbConnection connection) : base(connection)
        {
        }

        public override IEntityCommand GetAddCommand(DapperTestEntity entity)
        {
            throw new NotImplementedException();
        }

        public DapperTestEntity GetByCode(string sampleName)
        {
            throw new NotImplementedException();
        }

        public override DapperTestEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IEntityCommand GetDeleteCommand(DapperTestEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntityCommand GetUpdateCommand(DapperTestEntity entity)
        {
            return new UpdateDapperTestEntityCommand(
                _connection,
                entity);
        }
    }
}