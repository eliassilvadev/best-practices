﻿using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Domain.Repositories.Interfaces
{
    public interface IRepository<Entity> where Entity : IBaseEntity
    {
        bool Persist(Entity entity, IUnitOfWork unitOfWork);
        Entity GetById(Guid id);
    }
}
