﻿using Best.Practices.Core.Domain.Entities.Interfaces;

namespace Best.Practices.Core.Domain.Cqrs.CommandProviders
{
    public interface ICqrsCommandProvider<Entity> where Entity : IBaseEntity
    {
        IEntityCommand GetAddCommand(Entity entity);

        IEntityCommand GetUpdateCommand(Entity entity);

        IEntityCommand GetDeleteCommand(Entity entity);
        Task<Entity> GetById(Guid id);
    }
}