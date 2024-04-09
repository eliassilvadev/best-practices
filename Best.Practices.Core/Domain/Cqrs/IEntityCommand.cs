using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Domain.Cqrs
{
    public interface IEntityCommand
    {
        IBaseEntity AffectedEntity { get; }
        bool Execute();
    }
}