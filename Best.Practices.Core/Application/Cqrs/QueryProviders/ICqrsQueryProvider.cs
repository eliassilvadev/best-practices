namespace Best.Practices.Core.Application.Cqrs.QueryProviders
{
    public interface ICqrsQueryProvider<ResultOutput>
    {
        Task<ResultOutput> GetById(Guid id);
    }
}