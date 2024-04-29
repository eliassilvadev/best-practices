using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Domain.Repositories.Interfaces
{
    public interface IRepository<Entity> where Entity : IBaseEntity
    {
        void Persist(Entity entity, IUnitOfWork unitOfWork);
        Task<Entity> GetById(Guid id);
    }
}
