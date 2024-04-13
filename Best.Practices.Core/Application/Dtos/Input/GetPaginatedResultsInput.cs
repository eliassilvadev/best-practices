namespace Best.Practices.Core.Application.Dtos.Input
{
    public class GetPaginatedResultsInput
    {
        public IList<SearchFilterInput> Filters { get; set; }
        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }
    }
}