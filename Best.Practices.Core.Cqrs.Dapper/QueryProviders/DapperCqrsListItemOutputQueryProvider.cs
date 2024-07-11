using Best.Practices.Core.Application.Cqrs.QueryProviders;
using Best.Practices.Core.Application.Dtos.Input;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.QueryProviders
{
    public abstract class DapperCqrsListItemOutputQueryProvider<ResultOutput> : IListItemOutputCqrsQueryProvider<ResultOutput>
    {
        protected IDbConnection _connection;

        public DapperCqrsListItemOutputQueryProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public abstract Task<int> Count(IList<SearchFilterInput> filters);

        public abstract Task<IList<ResultOutput>> GetPaginatedResults(IList<SearchFilterInput> filters, int pageNumber, int itemsPerPage);
    }
}