using Best.Practices.Core.Application.Dtos.Input;

namespace Best.Practices.Core.Domain.Cqrs.QueryProvider
{
    public interface ICqrsQueryProvider<ResultOutput>
    {
        ResultOutput GetById(Guid id);
        int Count(IList<SearchFilterInput> filters);
        IList<ResultOutput> GetPaginatedResults(IList<SearchFilterInput> filters, int pageNumber, int itemsPerPage);
    }
}