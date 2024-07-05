using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Observer;

namespace Best.Practices.Core.Domain.Entities.Interfaces
{
    public interface IBaseEntity : IEntityObservable
    {
        Guid Id { get; }

        DateTime CreationDate { get; }

        EntityState State { get; }

        IBaseEntity EntityClone();

        void SetStateAsUpdated();

        void SetStateAsDeleted();

        void SetStateAsPersisted();

        void SetStateAsUnchanged();

        Dictionary<string, object> PersistedValues { get; }

        Dictionary<string, object> GetPropertiesToPersist();
    }
}