using Best.Practices.Core.Domain.Enumerators;

namespace Best.Practices.Core.Domain.Models.Interfaces
{
    public interface IBaseEntity
    {
        Guid Id { get; }

        DateTime CreationDate { get; }

        EntityState State { get; }

        IBaseEntity EntityClone();

        void SetStateAsUpdated();

        void SetStateAsDeleted();

        void SetStateAsPersisted();

        void SetStateAsUnchanged();

        Dictionary<string, object> GetPropertiesToPersist();
    }
}