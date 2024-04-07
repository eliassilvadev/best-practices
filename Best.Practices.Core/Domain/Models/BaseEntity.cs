using Best.Practices.Core.Domain.Enumerators;

namespace Best.Practices.Core.Domain.Models
{
    public abstract class BaseEntity
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

        public virtual void SetStateAsUpdated()
        {
            if ((State == EntityState.Unchanged) ||
                (State == EntityState.Persisted))
            {
                State = EntityState.Updated;
            }
        }

        public virtual void SetStateAsDeleted()
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
    }
}