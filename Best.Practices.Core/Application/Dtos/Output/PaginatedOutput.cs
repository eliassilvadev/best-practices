namespace Best.Practices.Core.Application.Dtos.Output
{
    public class PaginatedOutput<OutputType>
    {
        public int ActualPage { get; protected set; }

        public int MaxPage { get; protected set; }
        public int TotalResultsCount { get; protected set; }
        public IList<OutputType> ResultsInPage { get; protected set; }

        public PaginatedOutput(int actualPage, int maxPage, int totalResultsCount, IList<OutputType> resultsInPage)
        {
            ActualPage = actualPage;
            MaxPage = maxPage;
            ResultsInPage = resultsInPage;
            TotalResultsCount = totalResultsCount;
        }
    }
}