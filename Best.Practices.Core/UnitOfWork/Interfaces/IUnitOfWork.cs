using Best.Practices.Core.Domain.Cqrs;

namespace Best.Practices.Core.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void AddComand(IEntityCommand command);

        bool SaveChanges();

        void Rollback();

        void SetEntitiesPersistedState();
    }
}