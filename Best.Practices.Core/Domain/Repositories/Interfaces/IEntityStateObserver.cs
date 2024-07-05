using Best.Practices.Core.Domain.Entities.Interfaces;

namespace Best.Practices.Core.Domain.Repositories.Interfaces
{
    public interface IEntityStateObserver
    {
        T CreateEntityWihStateControl<T>(T entity) where T : IBaseEntity;
    }
}