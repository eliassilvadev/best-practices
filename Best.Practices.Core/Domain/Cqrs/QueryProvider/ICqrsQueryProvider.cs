using Best.Practices.Core.Application.Dtos.Input;

namespace Best.Practices.Core.Domain.Cqrs.QueryProvider
{
    public interface ICqrsQueryProvider<ResultOutput>
    {
        Task<ResultOutput> GetById(Guid id);
        Task<int> Count(IList<SearchFilterInput> filters);
        Task<IList<ResultOutput>> GetPaginatedResults(IList<SearchFilterInput> filters, int pageNumber, int itemsPerPage);
    }
}