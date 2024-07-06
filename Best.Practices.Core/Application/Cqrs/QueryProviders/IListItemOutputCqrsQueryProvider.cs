using Best.Practices.Core.Application.Dtos.Input;

namespace Best.Practices.Core.Application.Cqrs.QueryProviders
{
    public interface IListItemOutputCqrsQueryProvider<ResultOutput>
    {
        Task<int> Count(IList<SearchFilterInput> filters);
        Task<IList<ResultOutput>> GetPaginatedResults(IList<SearchFilterInput> filters, int pageNumber, int itemsPerPage);
    }
}