using Best.Practices.Core.Application.Cqrs.QueryProviders;
using Best.Practices.Core.Application.Dtos.Input;
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

        public abstract Task<int> Count(IList<SearchFilterInput> filters);

        public abstract Task<ResultOutput> GetById(Guid id);

        public abstract Task<IList<ResultOutput>> GetPaginatedResults(IList<SearchFilterInput> filters, int pageNumber, int itemsPerPage);
    }
}