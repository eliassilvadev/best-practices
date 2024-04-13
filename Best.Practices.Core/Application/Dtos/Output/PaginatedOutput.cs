namespace Best.Practices.Core.Application.Dtos.Output
{
    public class PaginatedOutput<OutputType>
    {
        public int ActualPage { get; protected set; }

        public int MaxPage { get; protected set; }
        public IList<OutputType> OutputResults { get; set; }

        public PaginatedOutput(int actualPage, int maxPage, IList<OutputType> outputResults)
        {
            ActualPage = actualPage;
            MaxPage = maxPage;
            OutputResults = outputResults;
        }
    }
}