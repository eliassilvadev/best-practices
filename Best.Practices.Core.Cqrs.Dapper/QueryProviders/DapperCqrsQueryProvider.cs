using Best.Practices.Core.Application.Cqrs.QueryProviders;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.QueryProviders
{
    public abstract class DapperCqrsQueryProvider<ResultOutput> : ICqrsQueryProvider<ResultOutput>
    {
        protected IDbConnection _connection;

        public DapperCqrsQueryProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public abstract Task<ResultOutput> GetById(Guid id);
    }
}