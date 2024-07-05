using Best.Practices.Core.Domain.Entities.Interfaces;

namespace Best.Practices.Core.Domain.Cqrs
{
    public interface IEntityCommand
    {
        IBaseEntity AffectedEntity { get; }
        Task<bool> ExecuteAsync();
    }
}