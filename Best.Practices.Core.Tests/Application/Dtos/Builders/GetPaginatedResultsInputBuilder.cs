using Best.Practices.Core.Application.Dtos.Input;

namespace Best.Practices.Core.Tests.Application.Dtos.Builders
{
    public class GetPaginatedResultsInputBuilder
    {
        private IList<SearchFilterInput> _filters;
        private int _pageNumber;
        private int _itemsPerPage;

        public GetPaginatedResultsInputBuilder()
        {
            _filters = [new SearchFilterInputBuilder().Build()];
            _pageNumber = 1;
            _itemsPerPage = 10;
        }

        public GetPaginatedResultsInputBuilder WithFilters(IList<SearchFilterInput> filters)
        {
            _filters = filters;
            return this;
        }

        public GetPaginatedResultsInputBuilder WithPageNumber(int pageNumber)
        {
            _pageNumber = pageNumber;
            return this;
        }

        public GetPaginatedResultsInputBuilder WithItemsPerPage(int itemsPerPage)
        {
            _itemsPerPage = itemsPerPage;
            return this;
        }

        public GetPaginatedResultsInput Build()
        {
            return new GetPaginatedResultsInput
            {
                Filters = _filters,
                PageNumber = _pageNumber,
                ItemsPerPage = _itemsPerPage
            };
        }
    }
}