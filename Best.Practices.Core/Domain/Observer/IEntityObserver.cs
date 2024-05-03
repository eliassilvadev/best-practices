using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Domain.Observer
{
    public interface IEntityObserver
    {
        void NotifyEntityPropertyUpdate(IBaseEntity entity, string propertyName, object propertyValue);
    }
}