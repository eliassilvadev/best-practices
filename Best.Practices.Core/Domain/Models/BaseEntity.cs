using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Extensions;

namespace Best.Practices.Core.Domain.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; protected set; }
        public EntityState State { get; protected set; }
        public DateTime CreationDate { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            State = EntityState.New;
        }

        public void SetStateAsUpdated()
        {
            if (State.In(EntityState.Unchanged, EntityState.Persisted))
            {
                State = EntityState.Updated;
            }
        }

        public void SetStateAsDeleted()
        {
            if (State != EntityState.New)
            {
                State = EntityState.Deleted;
            }
        }

        public void SetStateAsPersisted()
        {
            switch (State)
            {
                case EntityState.New:
                case EntityState.Updated:
                    {
                        State = EntityState.Persisted;
                        break;
                    };
                case EntityState.Deleted:
                    {
                        State = EntityState.PersistedDeleted;
                        break;
                    };
            };
        }

        public void SetStateAsUnchanged()
        {
            State = EntityState.Unchanged;
        }
        public virtual IBaseEntity EntityClone()
        {
            var cloneEntity = (BaseEntity)this.DeepClone();

            cloneEntity.Id = Guid.NewGuid();
            cloneEntity.CreationDate = DateTime.UtcNow;
            cloneEntity.State = EntityState.New;

            return cloneEntity;
        }

        public virtual object Clone()
        {
            return EntityClone();
        }
    }
}