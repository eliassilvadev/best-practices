using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Cqrs
{
    public interface IEntityCommand
    {
        IBaseEntity AffectedEntity { get; }
        bool Execute();
    }
}